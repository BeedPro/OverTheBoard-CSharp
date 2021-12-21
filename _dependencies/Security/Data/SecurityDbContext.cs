using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace OverTheBoard.Core.Security.Data
{
    public class SecurityDbContext : IdentityDbContext<OverTheBoardUser>
    {
        public string DbPath { get; private set; }
        
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options)
            : base(options)
        {
            var path = Environment.CurrentDirectory;
            DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}Data{System.IO.Path.DirectorySeparatorChar}SecurityDb.db";
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite($"Data Source={DbPath}");


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
        public DbSet<OverTheBoardUser> OverTheBoardUsers { get; set; }


    }
}
