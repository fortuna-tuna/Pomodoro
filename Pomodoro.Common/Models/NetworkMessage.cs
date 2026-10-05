using Pomodoro.Common.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.Common.Models
{
    public class NetworkMessage
    {
        public MessageType Type { get; set; }
        public string? Payload { get; set; }
    }
}
