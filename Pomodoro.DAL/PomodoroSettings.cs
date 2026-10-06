using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.DAL
{
    public class PomodoroSettings
    {
        public int Id { get; set; }
        public int PomodoroDuration { get; set; }
        public int ShortBreak { get; set; }
        public int LongBreak { get; set; }
        public int CountPomodoroBeforeLongBreak { get; set; }

        public int UserProfileId { get; set; }
        public UserProfile? UserProfile { get; set; }
    }
}
