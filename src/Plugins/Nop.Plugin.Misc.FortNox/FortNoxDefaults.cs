using Nop.Core;

namespace Nop.Plugin.Misc.FortNox
{
    /// <summary>
    /// Represents plugin constants
    /// </summary>
    public static class FortNoxDefaults
    {
        /// <summary>
        /// Gets a plugin system name
        /// </summary>
        public static string SystemName => "Misc.FortNox";

        /// <summary>
        /// Generic attribute name to hide general settings block on the plugin configuration page
        /// </summary>
        public static string HideGeneralBlock = "FortNoxPage.HideGeneralBlock";

        /// <summary>
        /// Gets a name of the synchronization schedule task
        /// </summary>
        public static string SynchronizationTaskName => "Customer - Synchronization (FortNox plugin)";

        /// <summary>
        /// Gets a type of the synchronization schedule task
        /// </summary>
        public static string SynchronizationTask => "Nop.Plugin.Misc.FortNox.Services.SynchronizationTask";

        /// <summary>
        /// Gets a default synchronization period in hours
        /// </summary>
        public static int DefaultSynchronizationPeriod => 12;

        /// <summary>
        /// Gets a default synchronization limit of Lists
        /// </summary>
        public static int DefaultSynchronizationListsLimit => 50;


    }
}