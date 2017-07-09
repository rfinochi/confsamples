using System;
using System.Text;
using System.Threading;

using Microsoft.Azure.Devices.Client;
using Newtonsoft.Json;

namespace SimulatedDevice
{
    class Program
    {
        private static DeviceClient deviceClient;
        private static string iotHubUri = "fnord.azure-devices.net";
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
            Random rand = new Random( );

            while ( true )
            {
                double data = 10 + rand.NextDouble( ) * 4 - 2;

                /*
                var telemetryDataPoint = new
                {
                    batteryMeasuredVoltage= data,
                    batteryMaxVoltage = 7800000,
                    touchSensorIsPressed = false,
                    colorSensorReflectedLightIntensity = 93,
                    colorSensorAmbientLightIntensity = 22,
                    colorSensorColor = 6,
                    ultrasonicSensorDistanceCentimeters = 44, 
                    soundSensorSoundPressure = data
                };

                var messageString = JsonConvert.SerializeObject( telemetryDataPoint );
                */

                var messageString = "{\"batteryMeasuredVoltage\":2647066,\"batteryMaxVoltage\":7500000,\"touchSensorIsPressed\":false,\"colorSensorReflectedLightIntensity\":93,\"colorSensorAmbientLightIntensity\":22,\"colorSensorColor\":5,\"ultrasonicSensorDistanceCentimeters\":44.3,\"soundSensorSoundPressure\":0}";
                var message = new Message( Encoding.ASCII.GetBytes( messageString ) );

                await deviceClient.SendEventAsync( message );
                Console.WriteLine( "{0} > Sending message: {1}", DateTime.Now, messageString );

                Thread.Sleep( 1000 );
            }
        }
    }
}