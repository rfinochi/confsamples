using Owin;
using Microsoft.Owin;
using System.Threading.Tasks;

namespace OwinHelloWorld
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Run(Invoke);
        }

        public Task Invoke(IOwinContext context)
        {
            context.Response.ContentType = "text/plain";
            return context.Response.WriteAsync("Hello World");
        }
    }
}