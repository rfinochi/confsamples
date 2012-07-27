using System;
using System.Web.Routing;

using SignalR;
using SignalR.Hosting.AspNet.Routing;

namespace EchoConnectionSignalR
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start( object sender, EventArgs e )
        {
            RouteTable.Routes.MapConnection<MyConnection>( "echo", "echo/{*operation}" );
        }
    }
}