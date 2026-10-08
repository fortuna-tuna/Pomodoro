using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.DAL
{
    public class Statistic
    {
        public int Id { get; set; }
        public int TotalCompletePomodoros { get; set; }
        public DateTime Day { get; set; }

        public int? UserProfileId { get; set; }
        public UserProfile? UserProfile { get; set; }
    }
}
