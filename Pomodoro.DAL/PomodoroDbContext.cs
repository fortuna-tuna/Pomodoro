using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pomodoro.DAL
{
    internal class PomodoroDbContext: DbContext
    {
        public DbSet<Statistic> Statistics { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<PomodoroSettings> PomodoroSettings { get; set; }
        public PomodoroDbContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(config.GetConnectionString("SqlClient"));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
