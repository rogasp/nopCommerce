using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Messages;
using Nop.Core.Domain.Orders;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Stores;
using Nop.Plugin.Misc.FortNox.Domain;
using Nop.Core.Domain.Common;

namespace Nop.Plugin.Misc.FortNox.Services
{
    /// <summary>
    /// Represents SendinBlue manager
    /// </summary>
    public class FortNoxManager
    {
        #region Fields

        private readonly IActionContextAccessor _actionContextAccessor;
        private readonly ICountryService _countryService;
        private readonly ICustomerService _customerService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILogger _logger;
        private readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
        private readonly ISettingService _settingService;
        private readonly IStateProvinceService _stateProvinceService;
        private readonly IStoreService _storeService;
        private readonly IUrlHelperFactory _urlHelperFactory;
        private readonly IWebHelper _webHelper;
        private readonly IWorkContext _workContext;
        private readonly FortNoxService _fortNoxService;
        private readonly IAddressService _addressService;

        #endregion

        #region Ctor

        public FortNoxManager(IActionContextAccessor actionContextAccessor,
            ICountryService countryService,
            ICustomerService customerService,
            IEmailAccountService emailAccountService,
            IGenericAttributeService genericAttributeService,
            ILogger logger,
            INewsLetterSubscriptionService newsLetterSubscriptionService,
            ISettingService settingService,
            IStateProvinceService stateProvinceService,
            IStoreService storeService,
            IUrlHelperFactory urlHelperFactory,
            IWebHelper webHelper,
            IWorkContext workContext,
            FortNoxService fortNoxService,
            IAddressService addressService)
        {
            _actionContextAccessor = actionContextAccessor;
            _countryService = countryService;
            _customerService = customerService;
            _emailAccountService = emailAccountService;
            _genericAttributeService = genericAttributeService;
            _logger = logger;
            _newsLetterSubscriptionService = newsLetterSubscriptionService;
            _settingService = settingService;
            _stateProvinceService = stateProvinceService;
            _storeService = storeService;
            _urlHelperFactory = urlHelperFactory;
            _webHelper = webHelper;
            _workContext = workContext;
            _fortNoxService = fortNoxService;
            _addressService = addressService;
        }

        #endregion

        #region Utilities


        #endregion

        #region Methods

        #region Synchronization

        /// <summary>
        /// Synchronize contacts 
        /// </summary>
        /// <param name="synchronizationTask">Whether it's a scheduled synchronization</param>
        /// <param name="storeId">Store identifier; pass 0 to synchronize contacts for all stores</param>
        /// <returns>List of messages</returns>
        public IList<(NotifyType Type, string Message)> Synchronize(bool synchronizationTask = true, int storeId = 0)
        {
            var messages = new List<(NotifyType, string)>();
            try
            {
                //whether plugin is configured
                var fortnoxSettings = _settingService.LoadSetting<FortnoxSettings>();
                if (string.IsNullOrEmpty(fortnoxSettings.AccessToken))
                    throw new NopException($"Plugin not configured");

                //use only passed store identifier for the manual synchronization
                //use all store ids for the synchronization task
                var noxCustomers = _fortNoxService.GetCustomers();

                foreach (var noxCustomer in noxCustomers)
                {
                    var syncedCustomer = _fortNoxService.GetForNoxCustomer(
                        Convert.ToInt32(noxCustomer.CustomerNumber));

                    if(syncedCustomer.IsSynced())
                    {

                    }
                    else
                    {
                        ///
                        /// Customer in FortNox is not synced.
                        /// Try to find customer in nopCommerce
                        ///

                        // Do FortNox have E-mail
                        if(noxCustomer.HasEmail())
                        {
                            // We have an e-mail
                            var customer = _fortNoxService.GetCustomerById(noxCustomer.CustomerNumber);

                            // See if we find the customer in nopCommerce
                            // We start with Email

                            var nopCustomer = _customerService.GetCustomerByEmail(customer.Email);

                            if(nopCustomer != null)
                            {
                                // Customer found
                                // Is this customer already synced
                                syncedCustomer = _fortNoxService.GetForNoxCustomer(
                                    Convert.ToInt32(nopCustomer.Id));
                                if(syncedCustomer.IsSynced())
                                {
                                    // Found customer in sync table.
                                    // Check what to do
                                }
                                else
                                {
                                    

                                }
                            }
                            else
                            {
                                // Customer not exists in nopCommerce
                                // Lets create the customer in nopCommerce
                                Customer _customer = new Customer();
                                Address _address = new Address();

                                _customer.Active = true;
                                _customer.CreatedOnUtc = DateTime.UtcNow;
                                _customer.RegisteredInStoreId = 1;
                                _address.CreatedOnUtc = DateTime.UtcNow;
                                
                                _customer.Email = _customer.Username = _address.Email = customer.Email;

                                var lastAndFirstName = customer.GetLastNameAndFirstName();
                                string lastName = lastAndFirstName.Item1;
                                string firstName = lastAndFirstName.Item2;

                                _address.Address1 = customer.Address1;
                                _address.Address2 = customer.Address2;
                                _address.City = customer.City;

                                if (customer.Type == FortnoxAPILibrary.Connectors.CustomerConnector.Type.COMPANY)
                                    _address.Company = customer.Name;
                                else
                                {
                                    _address.LastName = lastName;
                                    _address.FirstName = firstName;
                                }

                                _address.PhoneNumber = customer.Phone1;
                                _address.ZipPostalCode = customer.ZipCode;

                                _customerService.InsertCustomer(_customer);
                                _addressService.InsertAddress(_address);
                                _customer.Addresses.Add(_address);

                                //some validation
                                if (_address.CountryId == 0)
                                    _address.CountryId = null;
                                if (_address.StateProvinceId == 0)
                                    _address.StateProvinceId = null;
                                //customer.Addresses.Add(address);
                                _customer.CustomerAddressMappings.Add(new CustomerAddressMapping { Address = _address });
                                _customerService.UpdateCustomer(_customer);
                               

                                var registeredRole = _customerService.GetCustomerRoleBySystemName(NopCustomerDefaults.RegisteredRoleName);
                                _customer.AddCustomerRoleMapping(new CustomerCustomerRoleMapping { CustomerRole = registeredRole });

                                _customerService.UpdateCustomer(_customer);

                                FortNoxCustomerRecord fncr = new FortNoxCustomerRecord();
                                fncr.CustomerNumber = _customer.Id;
                                fncr.CustomerNumberNox = Convert.ToInt32(customer.CustomerNumber);
                                fncr.LastSyncDate = DateTime.UtcNow;
                                fncr.LastSyncedBy = "nopCommerce!";
                                fncr.OrganisationNumber = customer.OrganisationNumber;
                                fncr.Type = customer.Type.ToString();

                                _fortNoxService.AddSyncCustomer(fncr);
                            }

                        }
                        
                        // First we try and find customer by email.



                    }
                }

            }
            catch (Exception exception)
            {
                //log full error
                _logger.Error($"FortNox synchronization error: {exception.Message}.", exception, _workContext.CurrentCustomer);
                messages.Add((NotifyType.Error, $"FortNox synchronization error: {exception.Message}"));
            }

            return messages;
        }

        

        #endregion

        #endregion
    }
}