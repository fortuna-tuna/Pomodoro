using System;
using System.Collections.Generic;
using System.Text;
using Pomodoro.Common.Enum;

namespace Pomodoro.Common.Models
{
    public class TimeStateDTO
    {
        public TimerStates State { get; set; }
        public int StateDuration { get; set; }
    }
}
