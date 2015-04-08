using Microsoft.AspNet.Builder;

namespace TodoApi
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseWelcomePage();
        }
    }
}