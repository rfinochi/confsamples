using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.ServiceBus;
using System.ServiceModel;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            string account = GetAccount();

            Uri address = new Uri(String.Format("sb://{0}/services/{1}/EchoService/", ServiceBusEnvironment.DefaultRelayHostName, account));

            ServiceHost host = new ServiceHost(typeof(EchoService), address);

            host.Open();
            Console.WriteLine("Service address: " + address);
            Console.WriteLine("Press [Enter] to exit");
            Console.ReadLine();
            host.Close();

        }

        private static string GetAccount()
        {
            string account = Environment.GetEnvironmentVariable("SERVICEBUSLABS");
            while (string.IsNullOrEmpty(account))
            {
                Console.Write("Please enter the solution name to use in this lab:");
                account = Console.ReadLine();
            }

            return account;
        }
    }
}
