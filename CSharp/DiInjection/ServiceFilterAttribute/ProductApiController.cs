using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CSharp.DiInjection.ServiceFilterAttribute
{
    [CostomFilterAttribute]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : ControllerBase
    {
        
    }
}