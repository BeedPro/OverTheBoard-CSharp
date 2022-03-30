using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data.Entities;

namespace OverTheBoard.Data
{
    public class SecurityDbContext : IdentityDbContext<OverTheBoardUser>
    {
       
        public SecurityDbContext(DbContextOptions<SecurityDbContext> options)
            : base(options)
        {}
        
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("identity");
        }


        public DbSet<OverTheBoardUser> OverTheBoardUsers { get; set; }


    }
}
