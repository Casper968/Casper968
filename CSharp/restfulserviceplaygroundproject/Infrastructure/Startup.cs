using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace restfulserviceplaygroundproject.Infrastructure
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            StaticConfig = configuration;
            Environment = environment;
        }

        public static IConfiguration? StaticConfig { get; private set; }

        public static IWebHostEnvironment? Environment { get; private set; }
    }
}