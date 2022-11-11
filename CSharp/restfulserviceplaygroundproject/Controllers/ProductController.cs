using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restfulserviceplaygroundproject.Helpers;

namespace restfulserviceplaygroundproject.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        private readonly ProductDbContext _productDbContext;

        public ProductController(
            ILogger<ProductController> logger,
            ProductDbContext productDbContext)
        {
            _logger = logger;
            _productDbContext = productDbContext;
        }
            
        [HttpGet("Products")]
        public ActionResult GetProductLystAsync()
        {
            _productDbContext.SaveChanges();
            
            return Ok(_productDbContext.Product?.ToListAsync());
        }
    }
}