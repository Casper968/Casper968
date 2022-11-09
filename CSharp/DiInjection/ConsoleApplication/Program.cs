using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// By default. C# console application does not include dependency injection tools.
// First step install package
// Install-Package Microsoft.Extensions.DependencyInjection

namespace CSharp.DiInjection.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            // Now I have HostBuild to register HttpClientFactory in IoC
            var builder = new HostBuilder().ConfigureServices(
                (hostContext, services) => 
                {
                    services.AddHttpClient();
                }
            ).UseConsoleLifetime();

            var runner = builder.Build();
        }
    }
}