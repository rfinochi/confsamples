using System;
using System.Threading;

using Microsoft.AspNet.SignalR;

namespace TimeSignalR
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start( object sender, EventArgs e )
        {
            ThreadPool.QueueUserWorkItem( _ =>
            {
                var connectionContext = GlobalHost.ConnectionManager.GetConnectionContext<TimeConnection>( );

                while ( true )
                {
                    connectionContext.Connection.Broadcast( DateTime.Now.ToString( ) );
                    Thread.Sleep( 1000 );
                }
            } );
        }
    }
}