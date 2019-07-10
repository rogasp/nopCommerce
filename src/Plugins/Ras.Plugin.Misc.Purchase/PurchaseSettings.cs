using System;
using System.Collections.Generic;
using System.Text;
using Nop.Core.Configuration;

namespace Ras.Plugin.Misc.Purchase
{
    /// <summary>
    /// Represents a plugin settings
    /// </summary>
    public class PurchaseSettings : ISettings
    {
        public int DefaultDeliveryDays { get; set; }
        
    }
}
