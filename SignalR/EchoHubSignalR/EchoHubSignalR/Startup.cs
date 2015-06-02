using Microsoft.Owin;
using Owin;

[assembly: OwinStartup( typeof( EchoHubSignalR.Startup ) )]

namespace EchoHubSignalR
{
    public class Startup
    {
        public void Configuration( IAppBuilder app )
        {
            app.MapSignalR( );
        }
    }
}