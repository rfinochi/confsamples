using System;
using System.Linq;
using System.Threading;
using System.Web.Routing;

using LinqToTwitter;

using Microsoft.AspNet.SignalR;

namespace TweetsSignalR
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
                                                                       ConsumerKey = "EZfrBpOF1gFSW3uRmTIcoidbM",
                                                                       ConsumerSecret = "ZwGnEBFyWRZnpRNp95wrEOLEw3Ah3nGp70lNVXeI4vanww4u3O",
                                                                       AccessToken = "16930249-V14fiCkYRBwlSn6twcy4YUQ5yKL21dPhWn1FIHsk3",
                                                                       AccessTokenSecret = "351uV6pQZYRHZVMU0BSCU409bmtezaSTFXxGBrXyx5BbH"
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
                                                            connectionContext.Connection.Broadcast( tweets.Statuses.Select( t => t.Text ).ToList( ) );
                                                    }
                                                    Thread.Sleep( 5000 );
                                                }
                                            } );
        }
    }
}