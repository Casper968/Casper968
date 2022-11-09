using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.DiInjection.CustomFilterFactories
{
    public class CustomFilter : IActionFilter
    {
        private const int DefaultMaxRequestPerSecond = 3;

        private readonly IDistributedCache _cache;

        // This public field can let attribute pass param to overwrite defaultMaxRequestPerSecond
        public int MaxRequestPerSecond { get; set; } = DefaultMaxRequestPerSecond;

        public CustomFilter(IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
        
    }
}