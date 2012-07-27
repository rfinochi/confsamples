using System.Threading;

using SignalR.Hubs;

namespace ProgressSignalR
{
    [HubName( "progress" )]
    public class ProgressHub : Hub
    {
        public void StartProcessing( Person person )
        {
            Caller.notify( "We've started processing, " + person.Name );
            Clients.setProgress( 0 );

            for ( int i = 0; i <= 100; i++ )
            {
                Clients.setProgress( i );
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

            Caller.notify( "And we're done!" );
        }
    }
}