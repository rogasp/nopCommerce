using System;
using System.Linq;

namespace Nop.Plugin.Misc.FortNox.Domain
{
    /// <summary>
    /// FortNoxCustomerRecord extensions
    /// </summary>
    public static class FortNoxCustomerRecordExtensions
    {
        /// <summary>
        /// Gets a value indicating whether FortNoxCustomer is synced or not
        /// </summary>
        /// <param name="fortNoxCustomer">FortNoxCustomerRecord</param>
        /// <returns>Result</returns>
        public static bool IsSynced(this FortNoxCustomerRecord fortNoxCustomer)
        {
            if (fortNoxCustomer == null)
                return false;

            return true;
        }

        
    }
}