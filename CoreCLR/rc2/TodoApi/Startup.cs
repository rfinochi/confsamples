using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using TodoApi.Models;

namespace TodoApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<ITodoRepository, SqlTodoRepository>();
         }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseWelcomePage();
        }
    }
}
