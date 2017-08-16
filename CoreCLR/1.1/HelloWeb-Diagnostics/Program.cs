using System;
using System.Diagnostics;
using System.IO;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace HelloWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (Debugger.IsAttached)
            {
                Environment.SetEnvironmentVariable(
                    "ASPNETCORE_ENVIRONMENT", 
                    EnvironmentName.Development);
            }	    
	    
            var config = new ConfigurationBuilder()
                	  .AddJsonFile("hosting.json", optional: true)
        		  .AddEnvironmentVariables("ASPNETCORE_")
                	  .AddCommandLine(args)
                	  .Build();

            var host = new WebHostBuilder()
                        .UseKestrel()
                        .UseContentRoot(Directory.GetCurrentDirectory())
                        .UseConfiguration(config)
                        .UseIISIntegration()
                        .UseStartup<Startup>()
                        .Build();

            host.Run();
        }
    }
}
