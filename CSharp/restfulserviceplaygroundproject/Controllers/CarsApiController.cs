using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using restfulserviceplaygroundproject.DatabaseContext;
using restfulserviceplaygroundproject.Infrastructure;
using restfulserviceplaygroundproject.Model;
using restfulserviceplaygroundproject.Validation;

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

        [HttpPost("Brands")]
        public async Task<Result> AddCarBrands(CarBrand brand)
        {
            CreateCarBrandParamValidator validator = new CreateCarBrandParamValidator();
            ValidationResult vResult = validator.Validate(brand);
            if (!vResult.IsValid)
            {
                return Result.Fail(vResult.Errors.First().ToString());
            }

            this._carDbContext.WorldCarBrand.Add(brand);
            this._carDbContext.SaveChanges();

            return Result.Ok(this._carDbContext.WorldCarBrand.First(x => x.Name.Equals(brand.Name) && x.Url.Equals(brand.Url)));
        }

        [HttpGet("Models")]
        public async Task<Result> GetCarModels(string? brandName, string? name)
        {
            return Result.Ok(this._carDbContext.GetCarModels(brandName, name));
        }

        [HttpGet("Series")]
        public async Task<Result> GetCarSeries(string? brandName, string? series)
        {
            return Result.Ok(this._carDbContext.GetCarSeries(brandName, series));
        }

        [HttpGet("Version")]
        public async Task<Result> GetCarVersionList(string? brandName, string? series)
        {
            return Result.Ok(this._carDbContext.GetCarVersion(brandName, series));
        }
    }
}