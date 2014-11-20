using Owin;

namespace OwinHelloWorld.SelftHost
{
    class Startup
    {
        public void Configuration(IAppBuilder builder)
        {
            builder.Use(async (context, next) =>
            {
                if (context.Request.Headers["authorization"] == null)
                    context.Response.StatusCode = 401;
                else
                    await next();
            });

            builder.Use(async (context, next) =>
            {
                context.Response.ContentType = "text/plain";
                await context.Response.WriteAsync("Hello " + context.Request.Headers["authorization"]);
            });
        }
    }
}