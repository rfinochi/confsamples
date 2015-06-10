using System;
using System.Collections.Generic;

namespace ListServices
{
    public class Program
    {
        public Program(Microsoft.Framework.DependencyInjection.ServiceLookup.IServiceManifest serviceManifest)
        {
            ServiceManifest = serviceManifest;
        }

        Microsoft.Framework.DependencyInjection.ServiceLookup.IServiceManifest ServiceManifest
        {
            get;
        }

        public void Main(string[] args)
        {
            foreach (Type type in ServiceManifest.Services)
                Console.WriteLine(type.FullName);

            Console.ReadLine();
        }
    }
}

namespace Microsoft.Net.Runtime
{
    [AssemblyNeutral]
    [AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class AssemblyNeutralAttribute : Attribute
    {
    }
}

namespace Microsoft.Framework.DependencyInjection.ServiceLookup
{
    [Net.Runtime.AssemblyNeutral]
    public interface IServiceManifest
    {
        IEnumerable<Type> Services
        {
            get;
        }
    }
}