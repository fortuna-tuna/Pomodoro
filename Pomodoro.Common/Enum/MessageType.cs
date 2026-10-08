using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.Common.Enum
{
    public enum MessageType
    {
        // Client
        StartPomodoro = 101,
        StopPomodoro = 102,
        PausePomodoro = 103,
        SaveSettings = 104,

        // Server
        TimerUpdated = 201,
        ShowNotification = 202,
        ErrorResponce = 203

    }
}
