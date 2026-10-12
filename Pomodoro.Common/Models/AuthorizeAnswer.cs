using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.Common.Models
{
    public class AuthorizeAnswerDTO
    {
        public bool IsSuccess { get; set; }
        public PomodoroSettingsDTO profileSettings { get; set; }
    }
}
