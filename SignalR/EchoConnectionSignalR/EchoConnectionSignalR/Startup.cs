using Microsoft.Owin;
using Owin;

[assembly: OwinStartup( typeof( EchoConnectionSignalR.Startup ) )]

namespace EchoConnectionSignalR
{
    public class Startup
    {
        public void Configuration( IAppBuilder app )
        {
            app.MapSignalR<MyConnection>( "/echo" );
        }
    }
}
