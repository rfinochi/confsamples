using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using Nancy.Owin;

using TodoApi.Models;

namespace TodoApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ITodoRepository, MemoryTodoRepository>();
        }
        
        public void Configure(IApplicationBuilder app)
        {
            app.UseOwin(x => x.UseNancy());
        }
    }
}