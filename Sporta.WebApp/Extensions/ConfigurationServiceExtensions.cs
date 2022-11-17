using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Sporta.Data.CustomSql;
using Sporta.Data.Database.Sporta;
using Sporta.WebApp.Common;
using Sporta.WebApp.Hubs;
using Sporta.WebApp.Models;
using Sporta.WebApp.Repository;
using Sporta.WebApp.Services;
using Sporta.WebApp.Services.Interface;
using Sporta.WebApp.StoreProcedures;
using System;
using System.IO.Compression;

namespace Sporta.WebApp.Extensions
{
    public static class ConfigurationServiceExtensions
    {
        #region Configure Service's

        public static void ConfigureGeneralConfiguration(IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllersWithViews();
            services.AddResponseCaching();

            services.AddRazorPages();

            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.Configure<BrotliCompressionProviderOptions>(options =>
            {
                options.Level = CompressionLevel.Fastest;
            });
            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
                options.Providers.Add<BrotliCompressionProvider>();
            });


            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.LoginPath = "/Error/SessionExpired";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
            });
            services.Configure<ApplicationSettingsModel>(configuration.GetSection("ApplicationSettings"));
        }

        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IApplicationUserService, ApplicationUserService>();
            services.AddScoped<IDriveService, DriveService>();
            services.AddScoped<IDashboardService, DashboardService>();
            services.AddScoped<IGeneralService, GeneralService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<ICandidateService, CandidateService>();
            services.AddScoped<IResultService, ResultService>();
            services.AddScoped<IBaseStoredProc, BaseStoredProc>();
            services.AddSingleton(typeof(Utils));
            services.AddSignalR();
        }


        public static void ConfigureRepositories(this IServiceCollection services)
        {
            services.AddScoped(typeof(ISportaRepository<>), typeof(SportaRepository<>));
            services.AddScoped(typeof(ISportaRepositoryV2), typeof(SportaRepositoryV2));
        }


        public static void ConfigureDatabaseSQLContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<Sporta_DbContext>(Options =>
            {
                Options.UseSqlServer(
                    configuration.GetConnectionString("Sporta"),
                    sqlServerOptions => sqlServerOptions
                        .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                );
            });

            // For Custom Store procedure Calling
            services.AddDbContext<StoredProcedureClass>(Options =>
            {
                Options.UseSqlServer(
                    configuration.GetConnectionString("Sporta"),
                    sqlServerOptions => sqlServerOptions
                        .CommandTimeout((int)TimeSpan.FromMinutes(10).TotalSeconds)
                );
            });
        }

        #endregion       

        #region Middle Ware
        public static void MiddleWare(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.Use(async (ctx, next) =>
            {
                await next();
                if (ctx.Response.StatusCode == 404 && !ctx.Response.HasStarted)
                {
                    //Re-execute the request so the user gets the error page
                    var originalPath = ctx.Request.Path.Value;
                    ctx.Items["originalPath"] = originalPath;
                    ctx.Request.Path = "/Error/404";
                    ctx.Response.Redirect(ctx.Request.Path);
                    await next();
                }
            });

            app.UseResponseCompression();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error/Exception");
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
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<DashboardHub>("/dashboard");
            });
            
        }
        #endregion

    }
}
