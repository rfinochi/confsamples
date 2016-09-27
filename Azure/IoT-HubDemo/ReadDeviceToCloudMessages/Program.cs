using System;
using System.Text;
using System.Threading.Tasks;

using Microsoft.ServiceBus.Messaging;

namespace ReadDeviceToCloudMessages
{
    class Program
    {
        private static string connectionString = "";
        private static string iotHubD2cEndpoint = "messages/events";
        private static EventHubClient eventHubClient;

        static void Main( string[] args )
        {
            Console.WriteLine( "Receive messages\n" );
            eventHubClient = EventHubClient.CreateFromConnectionString( connectionString, iotHubD2cEndpoint );

            var d2cPartitions = eventHubClient.GetRuntimeInformation( ).PartitionIds;

            foreach ( string partition in d2cPartitions )
                ReceiveMessagesFromDeviceAsync( partition );

            Console.ReadLine( );
        }

        private async static Task ReceiveMessagesFromDeviceAsync( string partition )
        {
            var eventHubReceiver = eventHubClient.GetDefaultConsumerGroup( ).CreateReceiver( partition, DateTime.UtcNow );

            while ( true )
            {
                EventData eventData = await eventHubReceiver.ReceiveAsync( );
                if ( eventData == null )
                    continue;
                Console.WriteLine( string.Format( "Message received. Partition: {0} Data: '{1}'", partition, Encoding.UTF8.GetString( eventData.GetBytes( ) ) ) );
            }
        }
    }
}