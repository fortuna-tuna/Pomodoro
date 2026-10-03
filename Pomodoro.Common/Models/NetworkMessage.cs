using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.Common.Models
{
    internal class NetworkMessage
    {
        public int Type { get; set; }
        public string? Payload { get; set; }
    }
}
