using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.DiInjection.ServiceFilterAttribute
{
    public class StartUp
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            // Register our filter from the IoC container.
            services.AddScoped<CostomFilterAttribute>();
        }
    }
}