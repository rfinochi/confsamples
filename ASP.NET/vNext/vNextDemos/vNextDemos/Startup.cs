using System;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Http;
using Microsoft.AspNet.Routing;
using Microsoft.Data.Entity;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Framework.DependencyInjection;
using System.Reflection;

namespace vNextDemos
{
    public class Startup
    {
        public void Configure(IBuilder app)
        {
            // Enable Browser Link support
            app.UseBrowserLink();

            // Setup configuration sources
            var configuration = new Configuration();

            //configuration.AddIniFile("config.ini");
            configuration.AddJsonFile("config.json");
            configuration.AddEnvironmentVariables();

            //app.Run(async context =>
            //{
            //    await context.Response.WriteAsync(configuration.Get("Data:Configuration"));
            //});

            app.Run(async context =>
            {
                var asm = Assembly.Load(new AssemblyName("klr.host"));
                var assemblyVersion = asm.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

                await context.Response.WriteAsync(assemblyVersion.InformationalVersion);
            });

            // Set up application services
            app.UseServices(services =>
            {
                // Add EF services to the services container
                services.AddEntityFramework().AddInMemoryStore();
                services.AddScoped<PersonContext>();

                // Add MVC services to the services container
                services.AddMvc();
            });

            // Add static files to the request pipeline
            app.UseStaticFiles();

            // Add MVC to the request pipeline
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default", 
                    template: "{controller}/{action}/{id?}",
                    defaults: new { controller = "Home", action = "Index" });

                routes.MapRoute(
                    name: "api",
                    template: "{controller}/{id?}");
            });
        }
    }
}
