using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Configuration;

namespace Ras.Plugin.Misc.Fortnox
{
    /// <summary>
    /// Represents settings of the Fortnox plugin
    /// </summary>
    public class FortnoxSettings : ISettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to use sandbox (testing environment)
        /// </summary>
        public string AuthorizationCode { get; set; }
    }
}
