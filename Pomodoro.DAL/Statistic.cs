using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.DAL
{
    internal class Statistic
    {
        public int Id { get; set; }
        public int TotalCompletePomodoros { get; set; }
        public DateTime Day { get; set; }
    }
}
