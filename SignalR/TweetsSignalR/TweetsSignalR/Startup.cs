using Microsoft.Owin;

using Owin;

[assembly: OwinStartup( typeof( TweetsSignalR.Startup ) )]

namespace TweetsSignalR
{
    public class Startup
    {
        public void Configuration( IAppBuilder app )
        {
            app.MapSignalR<TweetsConnection>( "/tweets" );
        }
    }
}