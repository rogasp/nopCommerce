using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Services.Configuration;
using Nop.Services.Security;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Ras.Plugin.Misc.Fortnox.Models;

namespace Ras.Plugin.Misc.Fortnox.Controllers
{
    class MiscFortnoxController : BasePluginController
    {

        #region Fields

        private readonly IPermissionService _permissionService;
        private readonly ISettingService _settingService;
        private readonly IStoreContext _storeContext;

        #endregion

        #region Ctor

        public MiscFortnoxController(IPermissionService permissionService,
            ISettingService settingService,
            IStoreContext storeContext)
        {
            _permissionService = permissionService;
            _settingService = settingService;
            _storeContext = storeContext;
        }

        #endregion

        #region Methods

        [AuthorizeAdmin]
        [Area(AreaNames.Admin)]
        public IActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManagePaymentMethods))
                return AccessDeniedView();

            //load settings for a chosen store scope
            var storeScope = _storeContext.ActiveStoreScopeConfiguration;
            var fortnoxSettings = _settingService.LoadSetting<FortnoxSettings>(storeScope);

            var model = new ConfigurationModel
            {
                AuthorizationCode = fortnoxSettings.AuthorizationCode
            };

            if (storeScope <= 0)
                return View("~/Plugins/Misc.Fortnox/Views/Configure.cshtml", model);

            model.AuthorizationCode_OverrideForStore = _settingService.SettingExists(fortnoxSettings, x => x.AuthorizationCode, storeScope);
            
            return View("~/Plugins/Misc.PayPalStandard/Views/Configure.cshtml", model);
        }

        #endregion
    }
}
