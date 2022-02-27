using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data.Entities.Applications;

namespace OverTheBoard.Data
{
    public class ApplicationDbContext : DbContext
    {
        public string DbPath { get; private set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            var path = Environment.CurrentDirectory;
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}Data{System.IO.Path.DirectorySeparatorChar}OverTheBoardDb.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={DbPath}");
            options.EnableSensitiveDataLogging();
            
        }
         
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("application");

           //builder.Entity<GamePlayerEntity>()
            //    .HasOne(p => p.Game)
            //    .WithMany(b => b.Players)
            //    .HasForeignKey(p => p.PlayerId);
        }

        public DbSet<ChessGameEntity> Games { get; set; }
        public DbSet<GamePlayerEntity> Players { get; set; }

    }
}
