using Nop.Services.Tasks;

namespace Nop.Plugin.Misc.FortNox.Services
{
    /// <summary>
    /// Represents a schedule task to synchronize contacts
    /// </summary>
    public class SynchronizationTask : IScheduleTask
    {
        #region Fields

        private readonly FortNoxManager _fortNoxManager;

        #endregion

        #region Ctor

        public SynchronizationTask(FortNoxManager fortNoxManager)
        {
            _fortNoxManager = fortNoxManager;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Execute task
        /// </summary>
        public void Execute()
        {
            _fortNoxManager.Synchronize();
        }

        #endregion
    }
}