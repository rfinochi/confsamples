using System.Threading;

using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;

namespace ProgressSignalR
{
    [HubName( "progress" )]
    public class ProgressHub : Hub
    {
        public void StartProcessing( )
        {
            Clients.Caller.notify( "We've started processing" );
            Clients.Caller.setProgress( 0 );

            for ( int i = 0; i <= 100; i++ )
            {
                Clients.Caller.setProgress( i );
                if ( i <= 50 )
                {
                    Thread.Sleep( 100 );
                }
                else if ( i <= 60 )
                {
                    Thread.Sleep( 150 );
                }
                else if ( i <= 95 )
                {
                    Thread.Sleep( 50 );
                }
                else
                {
                    Thread.Sleep( 1000 );
                }
            }

            Clients.Caller.notify( "And we're done!" );
        }
    }
}