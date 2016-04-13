using System;
using System.Threading.Tasks;

using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;

namespace CreateDeviceIdentity
{
    class Program
    {
        private static RegistryManager registryManager;
        private static string connectionString = "";

        static void Main( string[] args )
        {
            registryManager = RegistryManager.CreateFromConnectionString( connectionString );
            AddDeviceAsync( ).Wait( );

            Console.ReadLine( );
        }

        private async static Task AddDeviceAsync( )
        {
            string deviceId = "Ev3";
            Device device;
            try
            {
                device = await registryManager.AddDeviceAsync( new Device( deviceId ) );
            }
            catch ( DeviceAlreadyExistsException )
            {
                device = await registryManager.GetDeviceAsync( deviceId );
            }
            Console.WriteLine( "Generated device key: {0}", device.Authentication.SymmetricKey.PrimaryKey );
        }
    }
}