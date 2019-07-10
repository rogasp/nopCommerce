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
        [NopResourceDisplayName("Plugins.Misc.FortNox.Fields.AccessToken")]
        public string AccessToken { get; set; }
        public bool AccessToken_OverrideForStore { get; set; }
        [NopResourceDisplayName("Plugins.Misc.FortNox.Fields.ClientId")]
        public string ClientId { get; set; }
        public bool ClientId_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FortNox.Fields.ClientSecret")]
        public string ClientSecret { get; set; }
        public bool ClientSecret_OverrideForStore { get; set; }


        [NopResourceDisplayName("Plugins.Misc.FortNox.Fields.OnlyCompanyCustomer")]
        public bool OnlyCompanyCustomer { get; set; }
        public bool OnlyCompanyCustomer_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FortNox.Fields.TermsOfPayment")]
        public string TermsOfPayment { get; set; }
        public IList<SelectListItem> AvailableTermsOfPayments { get; set; }
        public bool TermsOfPayment_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FortNox.Fields.DefaultPriceListCode")]
        public string DefaultPriceListCode { get; set; }
        public IList<SelectListItem> AvailablePriceListCodes { get; set; }
        public bool DefaultPriceListCode_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FortNox.Fields.SalesAccountSweden")]
        public string SalesAccountSweden { get; set; }
        public bool SalesAccountSweden_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FortNox.Fields.SalesAccountEUTaxed")]
        public string SalesAccountEUTaxed { get; set; }
        public bool SalesAccountEUTaxed_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FortNox.Fields.SalesAccountEUNotTaxed")]
        public string SalesAccountEUNotTaxed { get; set; }
        public bool SalesAccountEUNotTaxed_OverrideForStore { get; set; }

        [NopResourceDisplayName("Plugins.Misc.FortNox.Fields.SalesAccountNonEU")]
        public string SalesAccountNonEU { get; set; }
        public bool SalesAccountNonEU_OverrideForStore { get; set; }


        public bool HideGeneralBlock { get; set; }


        #endregion
    }
}