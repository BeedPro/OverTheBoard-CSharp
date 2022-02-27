using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OverTheBoard.Data;
using OverTheBoard.Data.Entities;
using OverTheBoard.Data.Repositories;

namespace Microsoft.Extensions.DependencyInjection
{
    [ExcludeFromCodeCoverage]
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSecurity(
            this IServiceCollection services)
        {

            var path = Environment.CurrentDirectory;
            var DbPath = $"{path}{System.IO.Path.DirectorySeparatorChar}Data{System.IO.Path.DirectorySeparatorChar}SecurityDb.db";

            services.AddDbContext<SecurityDbContext>(options =>
                options.UseSqlite(DbPath, b => b.MigrationsAssembly("OverTheBoard.Data")));
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(DbPath, b => b.MigrationsAssembly("OverTheBoard.Data")));

            
            //services.AddAuthentication(o =>
            //    {
            //        o.DefaultScheme = IdentityConstants.ApplicationScheme;
            //        o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            //    })
            //    .AddIdentityCookies(o => { });

            services.AddIdentity<OverTheBoardUser, IdentityRole>(options =>
                {
                    options.User.RequireUniqueEmail = false;
                })
                .AddEntityFrameworkStores<SecurityDbContext>()
                .AddDefaultTokenProviders();

            services.AddScoped(typeof(ISecurityRepository<>), typeof(SecurityRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            return services;
        }
    }
}