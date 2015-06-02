using Microsoft.Owin;
using Owin;

[assembly: OwinStartup( typeof( TimeSignalR.Startup ) )]

namespace TimeSignalR
{
    public class Startup
    {
        public void Configuration( IAppBuilder app )
        {
            app.MapSignalR<TimeConnection>( "/time" );
        }
    }
}
