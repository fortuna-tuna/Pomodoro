using Pomodoro.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.Common.Models
{
    public class NotificationDTO
    {
        public MessageType Type { get; set; }
        public string? NotificationText { get; set; }
    }
}
