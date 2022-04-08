using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using OverTheBoard.Infrastructure;
using OverTheBoard.Infrastructure.Queueing;
using OverTheBoard.Infrastructure.Services;
using OverTheBoard.ObjectModel;
using OverTheBoard.ObjectModel.Options;
using OverTheBoard.WebUI.BackgroundServices;
using OverTheBoard.WebUI.ModelPopulators;
using OverTheBoard.WebUI.SignalR;

namespace OverTheBoard.WebUI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<GameSettingOptions>(Configuration.GetSection("GameSettings"));
            services.Configure<EmailSettingOptions>(Configuration.GetSection("EmailSettings"));
            services.Configure<TournamentOptions>(Configuration.GetSection("Tournaments"));
            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllersWithViews();
            services.AddSecurity();
            services.AddScoped<IFileUploader, FileUploader>();
            services.AddSignalR();
            services.AddInfrastructure();
            services.AddHostedService<GameBackgroundService>();
            services.AddHostedService<GameInitialiserBackgroundService>();

            services.AddScoped<IBracketsViewModelPopulator, BracketsViewModelPopulator>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<UnrankedGameQueueHub>("/queue");
                endpoints.MapHub<GameMessageHub>("/piece-move");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
