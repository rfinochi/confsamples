using Microsoft.Owin;
using Owin;

[assembly: OwinStartup( typeof( Ev3RobotWatcher.Startup ) )]

namespace Ev3RobotWatcher
{
    public class Startup
    {
        public void Configuration( IAppBuilder app )
        {
            app.MapSignalR<RobotConnection>( "/robot" );
        }
    }
}