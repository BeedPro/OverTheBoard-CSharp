using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OverTheBoard.Infrastructure.Queueing;

namespace OverTheBoard.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a default implementation for the <see cref="T:Microsoft.AspNetCore.Http.IHttpContextAccessor" /> service.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddQueueing(
            this IServiceCollection services)
        {
            services.AddSingleton<IGameQueue, UnrankedGameQueue>();
            services.AddScoped<IQueueSelector, QueueSelector>();

            return services;
        }

        
    }
}
