using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Configuration;

namespace Nop.Plugin.Misc.FortNox
{
    /// <summary>
    /// Represents a plugin settings
    /// </summary>
    public class FortnoxSettings : ISettings
    {
        /// <summary>
        /// Gets or sets the Authorization code
        /// </summary>
        public string AuthorizationCode { get; set; }


    }
}
