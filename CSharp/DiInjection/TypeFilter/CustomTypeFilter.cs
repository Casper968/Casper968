using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.DiInjection.TypeFilter
{
    public class CustomTypeFilter : IActionFilter
    {
        private readonly IDistributedCache _cache;

        // Arguments can be supplied on the constructing.
        public int MaxRequestPerSecond { get; set; }
        
        public CustomTypeFilter(IDistributedCache cache, int maxRequestPerSecond)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            MaxRequestPerSecond = maxRequestPerSecond;
        }
    }
}