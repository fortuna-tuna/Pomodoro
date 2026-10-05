using System;
using System.Collections.Generic;
using System.Text;

namespace Server
{
    public enum TimerState
    {
        Stopped,
        Working,
        Paused,
        ShortBreak,
        LongBreak

    }

    internal class PomodoroTimer
    {
        object _lock = new();

        public TimerState State { get; set; }
        public int RemainingSeconds { get; private set; }
        public int ComplitetPomodoros { get; set; }

        public void Start(int minutes)
        {

        }

    }
}
