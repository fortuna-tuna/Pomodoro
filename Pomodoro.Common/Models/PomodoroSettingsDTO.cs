using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.Common.Models
{
    public class PomodoroSettingsDTO
    {
        public int PomodoroDuration { get; set; }
        public int ShortBreak { get; set; }
        public int LongBreak { get; set; }
        public int CountPomodoroBeforeLongBreak { get; set; }
    }
}
