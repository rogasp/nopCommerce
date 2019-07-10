using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortnoxAPILibrary;
using FortnoxAPILibrary.Connectors;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Directory;
using Nop.Plugin.Misc.FortNox.Domain;
using Nop.Services.Configuration;
using Nop.Services.Directory;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Orders;
using Nop.Services.Shipping;

namespace Nop.Plugin.Misc.FortNox.Services
{
    public class FortNoxService
    {
        #region Fields

        private readonly ISettingService _settingService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IStoreContext _storeContext;
        private readonly IRepository<FortNoxCustomerRecord> _fncRepository;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;

        #endregion

        #region Ctor

        public FortNoxService(ISettingService settingService,
            ILocalizationService localizationService,
            ILogger logger,
            IStoreContext storeContext,
            IRepository<FortNoxCustomerRecord> fncRepository,
            ICacheManager cacheManager,
            IEventPublisher eventPublisher)
        {
            _settingService = settingService;
            _localizationService = localizationService;
            _logger = logger;
            _storeContext = storeContext;
            _fncRepository = fncRepository;
            _cacheManager = cacheManager;
            _eventPublisher = eventPublisher;
        }

        #endregion

        #region Utilities



        #endregion

        #region Methods

        /// <summary>
        /// Insert a customer
        /// </summary>
        /// <param name="customer">Customer</param>
        public virtual void AddSyncCustomer(FortNoxCustomerRecord fortNoxCustomer)
        {
            if (fortNoxCustomer == null)
                throw new ArgumentNullException(nameof(fortNoxCustomer));

            _fncRepository.Insert(fortNoxCustomer);

            //event notification
            _eventPublisher.EntityInserted(fortNoxCustomer);
        }

        public FortNoxCustomerRecord GetForNoxCustomer(int customerID)
        {
            var query = from c in _fncRepository.Table
                        where c.CustomerNumberNox == customerID
                        select c;
            var customer = query.FirstOrDefault();

            return customer;
        }

        public IList<CustomerSubset> GetCustomers(bool OnlyCompanyCustomer = false)
        {
            //load settings for active store scope
            var storeId = _storeContext.ActiveStoreScopeConfiguration;
            var fortNoxSettings = _settingService.LoadSetting<FortnoxSettings>(storeId);

            try
            {
                var customerConnector = new CustomerConnector();
                customerConnector.ClientSecret = fortNoxSettings.ClientSecret;
                customerConnector.AccessToken = fortNoxSettings.AccessToken;
                var customers = customerConnector.Find();

                return customers.CustomerSubset;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public Customer GetCustomerById(string customerId)
        {
            //load settings for active store scope
            var storeId = _storeContext.ActiveStoreScopeConfiguration;
            var fortNoxSettings = _settingService.LoadSetting<FortnoxSettings>(storeId);

            try
            {
                var customerConnector = new CustomerConnector();
                customerConnector.ClientSecret = fortNoxSettings.ClientSecret;
                customerConnector.AccessToken = fortNoxSettings.AccessToken;
                var customer = customerConnector.Get(customerId);

                return customer;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public IList<PriceListSubset> GetPriceLists()
        {

            //load settings for active store scope
            var storeId = _storeContext.ActiveStoreScopeConfiguration;
            var fortNoxSettings = _settingService.LoadSetting<FortnoxSettings>(storeId);

            try
            {
                var priceListConnector = new PriceListConnector();
                priceListConnector.ClientSecret = fortNoxSettings.ClientSecret;
                priceListConnector.AccessToken = fortNoxSettings.AccessToken;
                var priceLists = priceListConnector.Find();

                return priceLists.PriceListSubset;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        public IList<TermsOfPaymentSubset> GetTermsOfPayments()
        {

            //load settings for active store scope
            var storeId = _storeContext.ActiveStoreScopeConfiguration;
            var fortNoxSettings = _settingService.LoadSetting<FortnoxSettings>(storeId);

            try
            {
                var termsOfPaymentConnector = new TermsOfPaymentConnector();
                termsOfPaymentConnector.ClientSecret = fortNoxSettings.ClientSecret;
                termsOfPaymentConnector.AccessToken = fortNoxSettings.AccessToken;
                var termsOfPayments = termsOfPaymentConnector.Find();

                return termsOfPayments.TermsOfPaymentSubset;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }

        }

        #endregion

    }
}
