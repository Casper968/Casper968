using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CSharp.DiInjection.CustomFilterFactories
{
    // MaxRequestPerSecond is public field to set value.
    // Parameter where decleare more clearly of its name and data type.
    [CustomFilter(MaxRequestPerSecond = 10)]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : ControllerBase
    {
        
    }
}