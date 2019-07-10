using System;
using Nop.Core;

namespace Nop.Plugin.Misc.FortNox.Domain
{
    /// <summary>
    /// Represents a customer record
    /// </summary>
    public partial class FortNoxCustomerRecord : BaseEntity
    {
        /// <summary>
        /// Gets or sets the store identifier
        /// </summary>
        public int CustomerNumberNox { get; set; }

        /// <summary>
        /// Gets or sets the warehouse identifier
        /// </summary>
        public int CustomerNumber { get; set; }

        /// <summary>
        /// Gets or sets the country identifier
        /// </summary>
        public string OrganisationNumber { get; set; }

        /// <summary>
        /// Gets or sets the state/province identifier
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the zip
        /// </summary>
        public DateTime LastSyncDate { get; set; }

        /// <summary>
        /// Gets or sets the shipping method identifier
        /// </summary>
        public string LastSyncedBy { get; set; }

    }
}