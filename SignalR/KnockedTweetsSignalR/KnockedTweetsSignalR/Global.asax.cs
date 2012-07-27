using System;
using System.Linq;
using System.Threading;
using System.Web.Routing;

using LinqToTwitter;

using SignalR;

namespace KnockedTweetsSignalR
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start( object sender, EventArgs e )
        {
            RouteTable.Routes.MapConnection<TweetsConnection>( "tweets", "tweets/{*operation}" );

            ThreadPool.QueueUserWorkItem( _ =>
                                            {
                                                var connectionContext = GlobalHost.ConnectionManager.GetConnectionContext<TweetsConnection>( );
                                                while ( true )
                                                {
                                                    using ( TwitterContext context = new TwitterContext( ) )
                                                    {
                                                        var tweets = context.Search.Where( t => t.Type == SearchType.Search && t.Query == "#fnord" )
                                                        .SingleOrDefault( ).Results;

                                                        connectionContext.Connection.Broadcast( tweets );
                                                    }
                                                    Thread.Sleep( 5000 );
                                                }
                                            } );
        }
    }
}