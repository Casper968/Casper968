using System.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CSharp.DiInjection.TypeFilter
{
    // Type filter does not need to be registered
    // This type filter can pass argument by constructor
    // However there is no guarantee safety type
    // Therefore not suggested
    [TypeFilter(typeof(CustomTypeFilter), Arguments = new object [] { 10 })]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : ControllerBase
    {
        
    }
}