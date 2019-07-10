using System;
using System.Collections.Generic;
using System.Text;
using FortnoxAPILibrary.Connectors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Messages;
using Nop.Plugin.Misc.FortNox.Models;
using Nop.Plugin.Misc.FortNox.Services;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Messages;
using Nop.Services.Stores;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.FortNox.Controllers
{
    public class FortNoxController : BasePluginController
    {
        #region Fields

        private readonly IEmailAccountService _emailAccountService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly ILogger _logger;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IMessageTokenProvider _messageTokenProvider;
        private readonly INotificationService _notificationService;
        private readonly ISettingService _settingService;
        private readonly IStaticCacheManager _cacheManager;
        private readonly IStoreContext _storeContext;
        private readonly IStoreMappingService _storeMappingService;
        private readonly IStoreService _storeService;
        private readonly IWorkContext _workContext;
        private readonly MessageTemplatesSettings _messageTemplatesSettings;
        private readonly FortNoxService _fortNoxService;


        #endregion

        #region Ctor

        public FortNoxController(IEmailAccountService emailAccountService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            ILogger logger,
            IMessageTemplateService messageTemplateService,
            IMessageTokenProvider messageTokenProvider,
            INotificationService notificationService,
            ISettingService settingService,
            IStaticCacheManager cacheManager,
            IStoreContext storeContext,
            IStoreMappingService storeMappingService,
            IStoreService storeService,
            IWorkContext workContext,
            MessageTemplatesSettings messageTemplatesSettings,
            FortNoxService fortNoxService)
        {
            _emailAccountService = emailAccountService;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _logger = logger;
            _messageTemplateService = messageTemplateService;
            _messageTokenProvider = messageTokenProvider;
            _notificationService = notificationService;
            _settingService = settingService;
            _cacheManager = cacheManager;
            _storeContext = storeContext;
            _storeMappingService = storeMappingService;
            _storeService = storeService;
            _workContext = workContext;
            _messageTemplatesSettings = messageTemplatesSettings;
            _fortNoxService = fortNoxService;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Prepare SendinBlueModel
        /// </summary>
        /// <param name="model">Model</param>
        private void PrepareModel(ConfigurationModel model)
        {
            //load settings for active store scope
            var storeId = _storeContext.ActiveStoreScopeConfiguration;
            var fortNoxSettings = _settingService.LoadSetting<FortnoxSettings>(storeId);

            //whether plugin is configured
            model.AvailableTermsOfPayments = new List<SelectListItem>();
            model.AvailablePriceListCodes = new List<SelectListItem>();

            if (string.IsNullOrEmpty(fortNoxSettings.AuthorizationCode))
            {
                return;
            }

            //prepare available options
            var termsOfPayments = _fortNoxService.GetTermsOfPayments();//= termsOfPaymentConnector.Find();
            var priceLists = _fortNoxService.GetPriceLists();


            foreach (var item in termsOfPayments)
            {
                model.AvailableTermsOfPayments.Add(new SelectListItem()
                {
                    Text = item.Description,
                    Value = item.Code,
                    Selected = (item.Code == fortNoxSettings.TermsOfPayment) ? true : false
                });
            }

            foreach (var item in priceLists)
            {
                model.AvailablePriceListCodes.Add(new SelectListItem()
                {
                    Text = item.Description,
                    Value = item.Code,
                    Selected = (item.Code == fortNoxSettings.DefaultPriceListCode) ? true : false
                });
                
            }

            //prepare common properties
            model.ActiveStoreScopeConfiguration = storeId;
            model.AuthorizationCode = fortNoxSettings.AuthorizationCode;
            model.AccessToken = fortNoxSettings.AccessToken;
            model.ClientId = fortNoxSettings.ClientId;
            model.ClientSecret = fortNoxSettings.ClientSecret;
            model.DefaultPriceListCode = fortNoxSettings.DefaultPriceListCode;
            model.TermsOfPayment = fortNoxSettings.TermsOfPayment;
            model.OnlyCompanyCustomer = fortNoxSettings.OnlyCompanyCustomer;
            model.SalesAccountEUNotTaxed = fortNoxSettings.SalesAccountEUNotTaxed;
            model.SalesAccountEUTaxed = fortNoxSettings.SalesAccountEUTaxed;
            model.SalesAccountSweden = fortNoxSettings.SalesAccountSweden;
            model.SalesAccountNonEU = fortNoxSettings.SalesAccountNonEU;

            model.HideGeneralBlock = _genericAttributeService.GetAttribute<bool>(_workContext.CurrentCustomer, FortNoxDefaults.HideGeneralBlock);


            //prepare overridable settings
            if (storeId > 0)
            {
                model.AuthorizationCode_OverrideForStore = _settingService.SettingExists(fortNoxSettings, settings => settings.AuthorizationCode, storeId);
                model.AccessToken_OverrideForStore = _settingService.SettingExists(fortNoxSettings, settings => settings.AccessToken, storeId);
                model.ClientId_OverrideForStore = _settingService.SettingExists(fortNoxSettings, settings => settings.ClientId, storeId);
                model.ClientSecret_OverrideForStore = _settingService.SettingExists(fortNoxSettings, settings => settings.ClientSecret, storeId);
                model.DefaultPriceListCode_OverrideForStore = _settingService.SettingExists(fortNoxSettings, settings => settings.DefaultPriceListCode, storeId);
                model.TermsOfPayment_OverrideForStore = _settingService.SettingExists(fortNoxSettings, settings => settings.TermsOfPayment, storeId);
                model.OnlyCompanyCustomer_OverrideForStore = _settingService.SettingExists(fortNoxSettings, settings => settings.OnlyCompanyCustomer, storeId);
                model.SalesAccountEUNotTaxed_OverrideForStore = _settingService.SettingExists(fortNoxSettings, settings => settings.SalesAccountEUNotTaxed, storeId);
                model.SalesAccountEUTaxed_OverrideForStore = _settingService.SettingExists(fortNoxSettings, settings => settings.SalesAccountEUTaxed, storeId);
                model.SalesAccountSweden_OverrideForStore = _settingService.SettingExists(fortNoxSettings, settings => settings.SalesAccountSweden, storeId);
                model.SalesAccountNonEU_OverrideForStore = _settingService.SettingExists(fortNoxSettings, settings => settings.SalesAccountNonEU, storeId);
                
            }
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            var model = new ConfigurationModel();
            PrepareModel(model);

            return View("~/Plugins/Misc.FortNox/Views/Configure.cshtml", model);
        }

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        [HttpPost, ActionName("Configure")]
        [FormValueRequired("save")]
        public IActionResult Configure(ConfigurationModel model)
        {
            if (!ModelState.IsValid)
                return Configure();

            var storeId = _storeContext.ActiveStoreScopeConfiguration;
            var fortNoxSettings = _settingService.LoadSetting<FortnoxSettings>(storeId);

            //save settings
            fortNoxSettings.AuthorizationCode = model.AuthorizationCode;
            fortNoxSettings.AccessToken = model.AccessToken;
            fortNoxSettings.ClientId = model.ClientId;
            fortNoxSettings.ClientSecret = model.ClientSecret;
            fortNoxSettings.DefaultPriceListCode = model.DefaultPriceListCode;
            fortNoxSettings.TermsOfPayment = model.TermsOfPayment;
            fortNoxSettings.OnlyCompanyCustomer = model.OnlyCompanyCustomer;
            fortNoxSettings.SalesAccountEUNotTaxed = model.SalesAccountEUNotTaxed;
            fortNoxSettings.SalesAccountEUTaxed = model.SalesAccountEUTaxed;
            fortNoxSettings.SalesAccountSweden = model.SalesAccountSweden;
            fortNoxSettings.SalesAccountNonEU = model.SalesAccountNonEU;


            /* We do not clear cache after each setting update.
             * This behavior can increase performance because cached settings will not be cleared 
             * and loaded from database after each update */
            _settingService.SaveSetting(fortNoxSettings, settings => settings.AuthorizationCode, clearCache: false);
            _settingService.SaveSetting(fortNoxSettings, settings => settings.AccessToken, clearCache: false);
            _settingService.SaveSetting(fortNoxSettings, settings => settings.ClientId, clearCache: false);
            _settingService.SaveSetting(fortNoxSettings, settings => settings.ClientSecret, clearCache: false);
            _settingService.SaveSetting(fortNoxSettings, settings => settings.DefaultPriceListCode, clearCache: false);
            _settingService.SaveSetting(fortNoxSettings, settings => settings.TermsOfPayment, clearCache: false);
            _settingService.SaveSetting(fortNoxSettings, settings => settings.OnlyCompanyCustomer, clearCache: false);
            _settingService.SaveSetting(fortNoxSettings, settings => settings.SalesAccountEUNotTaxed, clearCache: false);
            _settingService.SaveSetting(fortNoxSettings, settings => settings.SalesAccountEUTaxed, clearCache: false);
            _settingService.SaveSetting(fortNoxSettings, settings => settings.SalesAccountSweden, clearCache: false);
            _settingService.SaveSetting(fortNoxSettings, settings => settings.SalesAccountNonEU, clearCache: false);

            //now clear settings cache
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

            #endregion
        }
}
