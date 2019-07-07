using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.FortNox.Models
{
    /// <summary>
    /// Represents a configuration model
    /// </summary>
    public class ConfigurationModel : BaseNopModel
    {

        #region Ctor

        public ConfigurationModel()
        {
            
        }

        #endregion

        #region Properties

        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FortNox.Fields.AuthorizationCode")]
        public string AuthorizationCode { get; set; }
        public bool AuthorizationCode_OverrideForStore { get; set; }

        public bool HideGeneralBlock { get; set; }


        #endregion
    }
}