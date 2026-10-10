using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.Common.Models
{
    public class GetSettingsDTO
    {
        public string? Login { get; set; }
        public PomodoroSettingsDTO? Settings { get; set; }
    }
}
