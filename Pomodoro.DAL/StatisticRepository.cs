using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace Pomodoro.DAL
{
    internal class StatisticRepository
    {
        readonly PomodoroDbContext _context;
        public StatisticRepository(PomodoroDbContext context) => _context = context;
        public void AddNewObject(Statistic stat)
        {
            _context.Statistics.Add(stat);
        }
        
        public List<Statistic> ReadStatist(DateTime day)
        {
            return _context.Statistics.Where(x => x.Day.Date == day.Date).ToList();
        }
    }
}
