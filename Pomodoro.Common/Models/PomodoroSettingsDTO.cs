using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.Common.Models
{
    public class PomodoroSettingsDTO
    {
        public int PomodoroDuration { get; set; } = 25;
        public int ShortBreak { get; set; } = 5;
        public int LongBreak { get; set; } = 30;
        public int CountPomodoroBeforeLongBreak { get; set; } = 4;
    }
}
