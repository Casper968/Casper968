using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restfulserviceplaygroundproject.DatabaseContext;
using restfulserviceplaygroundproject.Infrastructure;

namespace restfulserviceplaygroundproject.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsApiController : ControllerBase
    {
        CarsDbContext _carDbContext;

        public CarsApiController(
            CarsDbContext carsDbContext)
        {
            this._carDbContext = carsDbContext;
        }

        [HttpGet("Brands")]
        public async Task<Result> GetCarBrands(string? name)
        {
            return Result.Ok(this._carDbContext.GetCarBrands(name));
        }

        [HttpGet("Models")]
        public async Task<Result> GetCarModels(string? brandName, string? name)
        {
            return Result.Ok(this._carDbContext.GetCarModels(brandName, name));
        }
    }
}