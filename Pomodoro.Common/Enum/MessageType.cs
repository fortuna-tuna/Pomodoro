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
        GetSettings = 105,
        AuthorizeRequest = 106,


        
        // Server
        ShowNotification = 201,
        ErrorResponce = 202,
        AuthorizeAnswer = 203,

        // Timer
        TimerChangeState = 301,
        TimerTick = 302,

    }
}
