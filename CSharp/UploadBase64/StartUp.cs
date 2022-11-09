using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.UploadBase64
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
            services.AddScoped<ImageFileUploadService>();
        }
    }
}