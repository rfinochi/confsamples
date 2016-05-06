using Microsoft.AspNet.Builder;

namespace HelloWorldDocker
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseWelcomePage();
        }
    }
}