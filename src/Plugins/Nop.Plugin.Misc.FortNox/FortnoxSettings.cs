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
        public string AccessToken { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public bool OnlyCompanyCustomer { get; set; }
        public string TermsOfPayment { get; set; }
        public string DefaultPriceListCode { get; set; }
        public string SalesAccountSweden { get; set; }
        public string SalesAccountEUTaxed { get; set; }
        public string SalesAccountEUNotTaxed { get; set; }
        public string SalesAccountNonEU { get; set; }


    }
}
