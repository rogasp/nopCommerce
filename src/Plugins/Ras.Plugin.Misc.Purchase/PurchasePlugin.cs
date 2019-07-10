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

namespace Ras.Plugin.Misc.Purchase
{
    public class PurchasePlugin : BasePlugin, IAdminMenuPlugin
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

        public PurchasePlugin(IEmailAccountService emailAccountService,
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
        /// Get configuration url
        /// </summary>

        public override string GetConfigurationPageUrl()
        {
            return $"{_webHelper.GetStoreLocation()}Admin/Purchase/Configure";
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override void Install()
        {
            // Settings
            _settingService.SaveSetting(new PurchaseSettings
            {
                DefaultDeliveryDays = 0
            });
            ;

            // Locals
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Purchase.Fileds.DefaultDeliveryDays", 
                "Default delivery time in days");

            base.Install();
        }

        /// <summary>
        /// Un install the plugin
        /// </summary>
        public override void Uninstall()
        {
            // Settings
            _settingService.DeleteSetting<PurchaseSettings>();
            // Locals
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Purchase.Fields.DefaultDeliveryDays");

            base.Uninstall();
        }


        /// <param name="rootNode"></param>
        public void ManageSiteMap(SiteMapNode rootNode)
        {
            var menuItem = new SiteMapNode()
            {
                Title = "Retail Managment System",
                ControllerName ="PurchaseController",
                ActionName = "Manage",
                Visible = true,
                RouteValues = new RouteValueDictionary() { { "area", "Admin"} },
            };

            var pluginNode = rootNode.ChildNodes.FirstOrDefault(x => x.SystemName == "Third party plugin");
            if (pluginNode != null)
                pluginNode.ChildNodes.Add(menuItem);
            else
                rootNode.ChildNodes.Add(menuItem);

        }
    }
}
