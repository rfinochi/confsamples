using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;
using System.Threading;

namespace SimulatedDevice
{
    class Program
    {
        private static DeviceClient deviceClient;
        private static string iotHubUri = "Ev3.azure-devices.net";
        private static string deviceKey = "";

        static void Main( string[] args )
        {
            Console.WriteLine( "Simulated device\n" );
            deviceClient = DeviceClient.Create( iotHubUri, new DeviceAuthenticationWithRegistrySymmetricKey( "Ev3", deviceKey ) );

            SendDeviceToCloudMessagesAsync( );
            Console.ReadLine( );
        }

        private static async void SendDeviceToCloudMessagesAsync( )
        {
            double avgWindSpeed = 10;
            Random rand = new Random( );

            while ( true )
            {
                double currentWindSpeed = avgWindSpeed + rand.NextDouble( ) * 4 - 2;

                var telemetryDataPoint = new
                {
                    deviceId = "Ev3",
                    windSpeed = currentWindSpeed
                };
                var messageString = JsonConvert.SerializeObject( telemetryDataPoint );
                var message = new Message( Encoding.ASCII.GetBytes( messageString ) );

                await deviceClient.SendEventAsync( message );
                Console.WriteLine( "{0} > Sending message: {1}", DateTime.Now, messageString );

                Thread.Sleep( 1000 );
            }
        }
    }
}