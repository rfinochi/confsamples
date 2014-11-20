using System;
using Microsoft.Owin.Hosting;

namespace OwinHelloWorld.SelftHost
{
    class Program
    {
        static void Main(string[] args)
        {
            using (WebApp.Start<Startup>("http://localhost:9003"))
            {
                Console.WriteLine("Running in http://localhost:9003");
                Console.WriteLine("Press key to quit");
                Console.ReadLine();
            }
        }
    }
}