using Microsoft.Owin;
using Owin;

[assembly: OwinStartup( typeof( ProgressSignalR.Startup ) )]

namespace ProgressSignalR
{
    public class Startup
    {
        public void Configuration( IAppBuilder app )
        {
            app.MapSignalR( );
        }
    }
}
