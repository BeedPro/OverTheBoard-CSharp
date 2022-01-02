using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data.Entities;

namespace OverTheBoard.Data
{
    public class SecurityDbContext : IdentityDbContext<OverTheBoardUser>
    {
        public string DbPath { get; private set; }
        
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options)
            : base(options)
        {
            var path = Environment.CurrentDirectory;
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}Data{System.IO.Path.DirectorySeparatorChar}OverTheBoardDb.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");

        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("identity");
        }


        public DbSet<OverTheBoardUser> OverTheBoardUsers { get; set; }


    }
}
