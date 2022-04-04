using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using OverTheBoard.Infrastructure.Common;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.Infrastructure.Tournaments;
using OverTheBoard.Infrastructure.Tournaments.Processors;

namespace OverTheBoard.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds a default implementation for the <see cref="T:Microsoft.AspNetCore.Http.IHttpContextAccessor" /> service.
        /// </summary>
        /// <param name="services">The <see cref="T:Microsoft.Extensions.DependencyInjection.IServiceCollection" />.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddInfrastructure(
            this IServiceCollection services)
        {
            services.AddScoped(typeof(ILocatorService<,>), typeof(LocatorService<,>));
            services.AddSingleton<IUnrankedGameQueue, UnrankedGameQueue>();
            services.AddScoped<IGameCompletionQueue, GameCompletionQueue>();
            services.AddScoped<ITournamentQueue, TournamentQueue>();

            //services.AddScoped<IQueueSelector, QueueSelector>();
            services.AddScoped<IUserService, UserService>();

            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ITournamentService, TournamentService>();

            services.AddScoped<IEloService, EloService>();

            //Register Email services
            services.AddScoped<IAccountEmailService, AccountEmailService>();
            services.AddScoped<ITournamentEmailService, TournamentEmailService>();

            //League/Ranking Locators
            services.AddScoped<IGameBackgroundService, GameOutcomeChecker>();
            services.AddScoped<IGameBackgroundService, GameCompletionQueueReceiver>();
            services.AddScoped<IGameBackgroundService, TournamentQueueReceiver>();

            // League / Ranking Locators
            services.AddScoped<IGameCompletionProcessor, UnrankingGameCompletionProcessor>();
            services.AddScoped<IGameCompletionProcessor, TournamentGameCompletionProcessor>();

            return services;
        }

        
    }
}
