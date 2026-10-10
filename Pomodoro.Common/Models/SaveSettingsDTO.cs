using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.Common.Models
{
    public class SaveSettingsDTO
    {
        public PomodoroSettingsDTO PomodoroSettings { get; set; }
        public int UserId { get; set; }
    }
}
