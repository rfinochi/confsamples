using Owin;

namespace OwinHelloWorld.SelftHost
{
    class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            builder.Use(async (context, next) =>
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Hello Word");
            });
        }
    }
}
