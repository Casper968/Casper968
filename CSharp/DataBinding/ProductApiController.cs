using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CSharp.Database.AddDatabaseContext;

namespace CSharp.DataBinding
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductApiController : ControllerBase
    {
        private ProductDbContext _productDbContext;
        public ProductApiController(ProductDbContext productDbContext)
        {
            this._productDbContext = productDbContext;
        }

        // not limited http method can use FromQuery attribute
        [HttpGet("GetListFromQuery")]
        public async ActionResult<List<ProductItem>> Get([FromQuery]string name)
        {
            List<ProductItem> result = await this._productDbContext.ProductList.Where(x => x.Name.Contains(name)).ToListAsync();
            return result;
        }

        // since read FromBody, can only use HttpPost or HttpPut attribute with form body
        [HttpPost("GetListFromBody")]
        public async ActionResult<List<ProductItem>> Get([FromBody]string name)
        {
            List<ProductItem> result = await this._productDbContext.ProductList.Where(x => x.Name.Contains(name)).ToListAsync();
            return result;
        }

        // not limited http method can use FromRoute attribute
        [HttpGet("GetListFromRoute/{name}")]
        public async ActionResult<List<ProductItem>> Get([FromRoute]string name)
        {
            List<ProductItem> result = await this._productDbContext.ProductList.Where(x => x.Name.Contains(name)).ToListAsync();
            return result;
        }

        // not limited http method can use FromHeader attribute
        [HttpGet("GetListFromHeader")]
        public async ActionResult<List<ProductItem>> Get([FromHeader]string name)
        {
            List<ProductItem> result = await this._productDbContext.ProductList.Where(x => x.Name.Contains(name)).ToListAsync();
            return result;
        }
    }
}