using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sporta.WebApp.Extensions;


namespace Sporta.WebApp
{
    public class Startup
    {
        /// <summary>
        /// Start up Constructor
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use1 this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            //Injection ApplicationSettings and other general configurations
            ConfigurationServiceExtensions.ConfigureGeneralConfiguration(services, Configuration);

            //Injecting Repositories
            ConfigurationServiceExtensions.ConfigureRepositories(services);

            //Injecting Services
            ConfigurationServiceExtensions.ConfigureServices(services);

            //Injecting DB Context
            ConfigurationServiceExtensions.ConfigureDatabaseSQLContext(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // Register Middle Ware //
            ConfigurationServiceExtensions.MiddleWare(app, env);
        }
    }
}
