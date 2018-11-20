using System;
using System.Diagnostics.CodeAnalysis;
using System.Timers;

namespace Utils
{
    public static class Timer
    {

        private static Action _CurrentAction;
        /// <summary>
        /// wait a millisecond time before execute action
        /// </summary>
        /// <param name="millisecond"></param>
        /// <param name="action"></param>
        public static void DelayAction(int millisecond, Action action)
        {
            _CurrentAction = action;
            System.Timers.Timer aTimer = new System.Timers.Timer();
            aTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            aTimer.Interval = millisecond;
            aTimer.Enabled = true;
        }

        /// <summary>
        /// Specify what you want to happen when the Elapsed event is raised.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="e"></param>
        [ExcludeFromCodeCoverage]
        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            _CurrentAction?.Invoke();
        }
    }
}
