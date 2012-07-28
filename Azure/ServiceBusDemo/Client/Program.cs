using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using System.ServiceModel;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter the name of the user you want to connect with: ");
            string serviceUserName = Console.ReadLine();

            Uri serviceUri = new Uri(String.Format("sb://{0}/services/{1}/EchoService/", ServiceBusEnvironment.DefaultRelayHostName, serviceUserName));

            EndpointAddress address = new EndpointAddress(serviceUri);

            ChannelFactory<IEchoChannel> channelFactory = new ChannelFactory<IEchoChannel>("RelayEndpoint", address);
            IEchoChannel channel = channelFactory.CreateChannel();
            channel.Open();

            Console.WriteLine("Enter text to echo (or [Enter] to exit):");
            string input = Console.ReadLine();
            while (!String.IsNullOrEmpty(input))
            {
                try
                {
                    Console.WriteLine("Server echoed: {0}", channel.Echo(input));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }
                input = Console.ReadLine();
            }

            channel.Close();
            channelFactory.Close();
        }
    }
}
