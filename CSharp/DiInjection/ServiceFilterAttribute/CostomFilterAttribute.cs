using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp.DiInjection.ServiceFilterAttribute
{
    public class CostomFilterAttribute
    {
        private readonly IDistributedCache _cache;

        public CostomFilterAttribute(IDistributeCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }
    }
}