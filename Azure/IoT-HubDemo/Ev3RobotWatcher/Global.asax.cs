using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

using Microsoft.AspNet.SignalR;
using Microsoft.ServiceBus.Messaging;
using Newtonsoft.Json;

namespace Ev3RobotWatcher
{
    public class Global : HttpApplication
    {
        private static string connectionString = "";
        private static string iotHubD2cEndpoint = "messages/events";
        private static EventHubClient eventHubClient;
                                                          
        protected void Application_Start( object sender, EventArgs e )
        {
            ThreadPool.QueueUserWorkItem( _ =>
            {
                eventHubClient = EventHubClient.CreateFromConnectionString( connectionString, iotHubD2cEndpoint );

                var d2cPartitions = eventHubClient.GetRuntimeInformation( ).PartitionIds;

                foreach ( string partition in d2cPartitions )
                    ReceiveMessagesFromDeviceAsync( partition );
            } );
        }

        private async static Task ReceiveMessagesFromDeviceAsync( string partition )
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup( ).CreateReceiver( partition, DateTime.UtcNow );
            var connectionContext = GlobalHost.ConnectionManager.GetConnectionContext<RobotConnection>( );

            while ( true )
            {
                EventData eventData = await eventHubReceiver.ReceiveAsync( );
                if ( eventData == null )
                    continue;

                connectionContext.Connection.Broadcast( Encoding.UTF8.GetString( eventData.GetBytes( ) ) );
            }
        }
    }
}