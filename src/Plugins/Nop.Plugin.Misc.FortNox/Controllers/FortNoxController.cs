using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Messages;
using Nop.Plugin.Misc.FortNox.Models;
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
            MessageTemplatesSettings messageTemplatesSettings)
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
            if (string.IsNullOrEmpty(fortNoxSettings.AuthorizationCode))
                return;

            //prepare common properties
            model.ActiveStoreScopeConfiguration = storeId;
            model.AuthorizationCode = fortNoxSettings.AuthorizationCode;

            model.HideGeneralBlock = _genericAttributeService.GetAttribute<bool>(_workContext.CurrentCustomer, FortNoxDefaults.HideGeneralBlock);


            //prepare overridable settings
            if (storeId > 0)
            {
                model.AuthorizationCode_OverrideForStore = _settingService.SettingExists(fortNoxSettings, settings => settings.AuthorizationCode, storeId);
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

            //set API key
            fortNoxSettings.AuthorizationCode = model.AuthorizationCode;
            _settingService.SaveSetting(fortNoxSettings, settings => settings.AuthorizationCode, clearCache: false);
            _settingService.ClearCache();

            _notificationService.SuccessNotification(_localizationService.GetResource("Admin.Plugins.Saved"));

            return Configure();
        }

            #endregion
        }
}
