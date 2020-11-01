using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;

namespace DAL
{
    public class AppDbContext: DbContext
    {
        
        public DbSet<GameSettings> GameSettingses { get; set; } = default!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
          .UseSqlite(@"Data Source = C:\Users\zeka\RiderProjects\project\MineSweeper\WebApp\app.db");
        }
        

    }
    
}