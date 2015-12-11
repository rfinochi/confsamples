using Microsoft.AspNet.Builder;

namespace OwinHelloWorld
{
    public class Startup
    {
        public void Configure(IApplicationBuilder app)
        {
            app.UseWelcomePage();
        }
    }
}