using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup( typeof( KnockedTweetsSignalR.Startup ) )]

namespace KnockedTweetsSignalR
{
    public class Startup
    {
        public void Configuration( IAppBuilder app )
        {
            app.MapSignalR<TweetsConnection>( "/tweets" );
        }
    }
}
