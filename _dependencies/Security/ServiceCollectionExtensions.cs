using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
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
            var buildServiceProvider = services.BuildServiceProvider();
            var option = buildServiceProvider.GetServices<IOptions<DatabaseOptions>>()?.FirstOrDefault()?.Value ?? new DatabaseOptions();
            if (option.Provider?.Equals("sqlserver", StringComparison.InvariantCultureIgnoreCase) ?? false)
            {
                services.AddDatabaseSqlServer(option.ConnectionString);
            }
            else
            {
                services.AddDatabaseSqlLite();
            }


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

        private static IServiceCollection AddDatabaseSqlServer(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<SecurityDbContext>(options =>
                options.UseSqlServer(connectionString, b =>
                    b.MigrationsAssembly("OverTheBoard.Data")));

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("OverTheBoard.Data")));

            return services;
        }
        
        private static IServiceCollection AddDatabaseSqlLite(this IServiceCollection services)
        {
            
                var path = Environment.CurrentDirectory;
                var DbPath =
                    $"{path}{System.IO.Path.DirectorySeparatorChar}Data{System.IO.Path.DirectorySeparatorChar}OverTheBoardDb.db";

                services.AddDbContext<SecurityDbContext>(options =>
                    options.UseSqlite($"Data Source={DbPath}", b =>
                        b.MigrationsAssembly("OverTheBoard.Data")));

                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseSqlite($"Data Source={DbPath}", b => b.MigrationsAssembly("OverTheBoard.Data")));
            

            return services;
        }
    }
}