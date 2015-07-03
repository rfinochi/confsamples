using System;
using System.Linq;
using System.Threading;
using System.Web.Routing;

using LinqToTwitter;

using Microsoft.AspNet.SignalR;

namespace KnockedTweetsSignalR
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start( object sender, EventArgs e )
        {
            ThreadPool.QueueUserWorkItem( _ =>
                                            {
                                                var connectionContext = GlobalHost.ConnectionManager.GetConnectionContext<TweetsConnection>( );
                                                var auth = new SingleUserAuthorizer
                                                {
                                                    CredentialStore = new SingleUserInMemoryCredentialStore
                                                    {
                                                        ConsumerKey = "",
                                                        ConsumerSecret = "",
                                                        AccessToken = "",
                                                        AccessTokenSecret = ""
                                                    }
                                                };

                                                while ( true )
                                                {
                                                    using ( TwitterContext context = new TwitterContext( auth ) )
                                                    {
                                                        var tweets = ( from search in context.Search
                                                                       where search.Type == SearchType.Search && search.Query == "#fnord"
                                                                       select search ).SingleOrDefault( );

                                                        if ( tweets != null && tweets.Statuses != null )
                                                            connectionContext.Connection.Broadcast( tweets.Statuses.Select( t => t ).ToList( ) );
                                                    }
                                                    Thread.Sleep( 5000 );
                                                }

                                            } );
        }
    }
}