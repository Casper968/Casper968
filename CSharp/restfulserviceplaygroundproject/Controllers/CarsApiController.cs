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

            await this._carDbContext.WorldCarBrand.AddAsync(brand);
            await this._carDbContext.SaveChangesAsync();

            return Result.Ok(this._carDbContext.WorldCarBrand.First(x => x.Name.Equals(brand.Name) && x.Url.Equals(brand.Url)));
        }

        [HttpGet("Models")]
        public async Task<Result> GetCarModels(string? brandName, string? name)
        {
            return Result.Ok(this._carDbContext.GetCarModels(brandName, name));
        }

        [HttpPost("Models")]
        public async Task<Result> AddCarModels(CarModel model)
        {
            CreateCarSeriesParamValidator validator = new CreateCarSeriesParamValidator(this._carDbContext);
            ValidationResult vResult = validator.Validate(model);
            if (!vResult.IsValid)
            {
                return Result.Fail(vResult.Errors.First().ToString());
            }

            await this._carDbContext.WorldCarModel.AddAsync(model);
            await this._carDbContext.SaveChangesAsync();

            return Result.Ok(this._carDbContext.WorldCarModel.First(x => x.Name.Equals(model.Name) && x.Url.Equals(model.Url)));
        }

        [HttpGet("Series")]
        public async Task<Result> GetCarSeries(string? brandName, string? series)
        {
            return Result.Ok(this._carDbContext.GetCarSeries(brandName, series));
        }

        [HttpPost("Series")]
        public async Task<Result> AddCarModels(CarSeries series)
        {
            CreateCarSeriesParamValidator validator = new CreateCarSeriesParamValidator(this._carDbContext);
            ValidationResult vResult = validator.Validate(series);
            if (!vResult.IsValid)
            {
                return Result.Fail(vResult.Errors.First().ToString());
            }

            await this._carDbContext.WorldCarSeries.AddAsync(series);
            await this._carDbContext.SaveChangesAsync();

            return Result.Ok(this._carDbContext.WorldCarSeries.First(x => x.Name.Equals(series.Name) && x.Url.Equals(series.Url)));
        }

        [HttpGet("Version")]
        public async Task<Result> GetCarVersionList(string? brandName, string? series)
        {
            return Result.Ok(this._carDbContext.GetCarVersion(brandName, series));
        }
        
        [HttpPost("Version")]
        public async Task<Result> AddCarModels(CarVersion series)
        {
            CreateCarVersionParamValidator validator = new CreateCarVersionParamValidator(this._carDbContext);
            ValidationResult vResult = validator.Validate(series);
            if (!vResult.IsValid)
            {
                return Result.Fail(vResult.Errors.First().ToString());
            }

            await this._carDbContext.WorldCarVersion.AddAsync(series);
            await this._carDbContext.SaveChangesAsync();

            return Result.Ok(this._carDbContext.WorldCarVersion.First(x => x.Name.Equals(series.Name) && x.Url.Equals(series.Url)));
        }
    }
}