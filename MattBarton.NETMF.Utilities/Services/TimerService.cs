using System;
using System.Threading;

namespace MattBarton.NETMF.Utilities.Services
{
    public class TimerService : ITimerService
    {
        #region Fields

        private int _timeoutSeconds = 30;
        private DateTime _timeoutAt;

        #endregion

        #region Constructors

        public TimerService()
        {
            this._timeoutAt = DateTime.Now.AddSeconds(this._timeoutSeconds);
        }

        public TimerService(int timeoutSeconds) :base()
        {
            this._timeoutSeconds = timeoutSeconds;
            
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Has the set timeout been reached?
        /// </summary>
        /// <returns></returns>
        public bool TimeoutReached()
        {
            return DateTime.Now < this._timeoutAt ? false : true;
        }

        /// <summary>
        /// Perform a wait
        /// </summary>
        /// <param name="milliseconds"></param>
        public void Sleep(int milliseconds)
        {
            Thread.Sleep(milliseconds);
        }

        #endregion
    }
}
