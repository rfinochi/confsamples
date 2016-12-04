using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using TodoApi.Models;

namespace TodoApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddSingleton<ITodoRepository, DbTodoRepository>();
         }

        public void Configure(IApplicationBuilder app)
        {
            app.UseMvc();
            app.UseWelcomePage();
        }
    }
}