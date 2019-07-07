using System;
using System.Collections.Generic;
using System.Text;
using Nop.Services.Cms;
using Nop.Services.Common;
using Nop.Services.Configuration;
using Nop.Services.Localization;
using Nop.Services.Plugins;

namespace Ras.Plugin.Misc.Fortnox
{
    /// <summary>
    /// Represents the Fortnox plugin
    /// </summary>
    public class FortnoxPlugin : BasePlugin, IMiscPlugin
    {

        #region Fields

        private readonly ILocalizationService _localizationService;
        private readonly ISettingService _settingService;

        #endregion

        public FortnoxPlugin(ILocalizationService localizationService,
            ISettingService settingService)
        {
            _localizationService = localizationService;
            _settingService = settingService;
        }

        /// <summary>
        /// Install the plugin
        /// </summary>
        public override void Install()
        {
            //settings
            _settingService.SaveSetting(new FortnoxSettings
            {
                AuthorizationCode = ""
            });

            //locales
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Fields.AuthorizationCode", "Authorization code");
            _localizationService.AddOrUpdatePluginLocaleResource("Plugins.Misc.Fortnox.Instructions", @"
            <p>
	            <b>If you're using this gateway ensure that your primary store currency is supported by Fortnox.</b>
	            <br />
	            <br />
            </p>");
            
            base.Install();
        }

        /// <summary>
        /// Uninstall the plugin
        /// </summary>
        public override void Uninstall()
        {
            //settings
            _settingService.DeleteSetting<FortnoxSettings>();

            //locales
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Fields.AuthorizationCode");
            _localizationService.DeletePluginLocaleResource("Plugins.Misc.Fortnox.Instructions");

            base.Uninstall();
        }
    }
}
