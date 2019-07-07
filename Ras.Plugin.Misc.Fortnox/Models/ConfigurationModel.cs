using System;
using System.Collections.Generic;
using System.Text;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Ras.Plugin.Misc.Fortnox.Models
{
    /// <summary>
    /// Represents a configuration model
    /// </summary>
    public class ConfigurationModel : BaseNopModel
    {
        public int ActiveStoreScopeConfiguration { get; set; }

        [NopResourceDisplayName("Plugins.Misc.Fortnox.Fields.AuthorizationCode")]
        public string AuthorizationCode { get; set; }
        public bool AuthorizationCode_OverrideForStore { get; set; }

        
    }
}
