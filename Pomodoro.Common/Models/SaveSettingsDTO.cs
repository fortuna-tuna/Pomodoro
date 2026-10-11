using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.Common.Models
{
    public class SaveSettingsDTO
    {
        public string Login { get; set; }
        public PomodoroSettingsDTO PomodoroSettings { get; set; }
    }
}
