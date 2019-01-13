using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SLMM.Api;
using SLMM.Api.Interfaces;
using SLMM.WebApi.Model;

namespace SLMM.WebApi
{
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile($"gardenconfig.json", optional: false, reloadOnChange: false)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            var gardenConfig = new GardenConfig();
            _configuration.GetSection("GardenConfig").Bind(gardenConfig);

            var slmmLocation = new SlmmLocation();
            _configuration.GetSection("SLMMLocation").Bind(slmmLocation);

            services.AddSingleton<ISmartLawnMowingMachine>(
                new SmartLawnMowingMachine(
                    new Garden(gardenConfig.Width, gardenConfig.Length), 
                    slmmLocation.X, 
                    slmmLocation.Y, 
                    slmmLocation.Orientation));
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller=Slmm}/{action=Index}");
            });
        }
    }
}
