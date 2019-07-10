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
using Nop.Core.Domain.Tasks;
using Nop.Plugin.Misc.FortNox.Data;

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
        private readonly FortNoxCustomerContext _objectContext;

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
            WidgetSettings widgetSettings,
            FortNoxCustomerContext objectContext)
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
            _objectContext = objectContext;
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
                AuthorizationCode = "",
                AccessToken = "",
                ClientId = "",
                ClientSecret = "",
                DefaultPriceListCode = "",
                TermsOfPayment = "", 
                OnlyCompanyCustomer = false,
                SalesAccountEUNotTaxed = "",
                SalesAccountEUTaxed = "",
                SalesAccountNonEU = "",
                SalesAccountSweden = ""

            });

            //database objects
            _objectContext.Install();

            //install synchronization task
            if (_scheduleTaskService.GetTaskByType(FortNoxDefaults.SynchronizationTask) == null)
            {
                _scheduleTaskService.InsertTask(new ScheduleTask
                {
                    Enabled = false,
                    Seconds = FortNoxDefaults.DefaultSynchronizationPeriod * 60 * 60,
                    Name = FortNoxDefaults.SynchronizationTaskName,
                    Type = FortNoxDefaults.SynchronizationTask,
                });
            }


            //locales
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.AuthorizationCode", "Authorization code");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.ClientId", "Client Id");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.AccessToken", "Access Token");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.ClientSecret", "Client Secret");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.OnlyCompanyCustomer", "Only Company Customer");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.TermsOfPayment", "Terms Of Payment");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.DefaultPriceListCode", "Default Price List Code");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.SalesAccountSweden", "Sales Account Sweden");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.SalesAccountEUTaxed", "Sales Account EUT axed");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.SalesAccountEUNotTaxed", "Sales Account EU Not Taxed");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.SalesAccountNonEU", "Sales Account Non EU");
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

            //database objects
            _objectContext.Uninstall();

            //locales
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.AuthorizationCode");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.AccessToken");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.ClientId");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.ClientSecret");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.OnlyCompanyCustomer");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.TermsOfPayment");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.DefaultPriceListCode");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.SalesAccountSweden");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.SalesAccountEUTaxed");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.SalesAccountEUNotTaxed");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.SalesAccountNonEU");
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
