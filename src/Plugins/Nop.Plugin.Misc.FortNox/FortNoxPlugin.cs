using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Routing;
using Nop.Core;
using Nop.Core.Domain.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Services.Plugins;
using Nop.Services.Stores;
using Nop.Services.Tasks;
using Nop.Web.Framework.Menu;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.FortNox
{
    public class FortNoxPlugin : BasePlugin, IAdminMenuPlugin
    {
        #region Fields

        private readonly IEmailAccountService _emailAccountService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILocalizationService _localizationService;
        private readonly IMessageTemplateService _messageTemplateService;
        private readonly IScheduleTaskService _scheduleTaskService;
        private readonly ISettingService _settingService;
        private readonly IStoreService _storeService;
        private readonly IWebHelper _webHelper;
        private readonly WidgetSettings _widgetSettings;

        #endregion

        #region Ctor

        public FortNoxPlugin(IEmailAccountService emailAccountService,
            IGenericAttributeService genericAttributeService,
            ILocalizationService localizationService,
            IMessageTemplateService messageTemplateService,
            IScheduleTaskService scheduleTaskService,
            ISettingService settingService,
            IStoreService storeService,
            IWebHelper webHelper,
            WidgetSettings widgetSettings)
        {
            _emailAccountService = emailAccountService;
            _genericAttributeService = genericAttributeService;
            _localizationService = localizationService;
            _messageTemplateService = messageTemplateService;
            _scheduleTaskService = scheduleTaskService;
            _settingService = settingService;
            _storeService = storeService;
            _webHelper = webHelper;
            _widgetSettings = widgetSettings;
        }

        #endregion

        /// <summary>
        /// Gets a configuration page URL
        /// </summary>
        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/FortNox/Configure";
        }


        /// <summary>
        /// Install the plugin
        /// </summary>
        public override void Install()
        {
            //Settings
            _settingService.SaveSetting(new FortnoxSettings
            {
                AuthorizationCode = ""
            });

            //install synchronization task


            //locales
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.AuthorizationCode", "Authorization code");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.General", "General");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Instructions", @"
            <p>
	            <b>If you're using this gateway ensure that your primary store currency is supported by Fortnox.</b>
	            <br />
	            <br />
            </p>");

            base.Install();
        }

        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<FortnoxSettings>();

            //locales
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.AuthorizationCode");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.General");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Instructions");

            base.Uninstall();
        }

        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                //SystemName = "FortNox",
                Title = "FortNox",
                ControllerName = "ControllerName",
                ActionName = "Manage",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", "Admin" } },
            };
            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugins");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);
        }
    }
}
