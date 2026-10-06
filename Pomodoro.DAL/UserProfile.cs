using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.DAL
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public PomodoroSettings? Settings { get; set; }
        public List<Statistic> Statistics { get; set; } = new();
    }
}
