using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;
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
            return Result.Ok(await this._carDbContext.GetCarBrands(name));
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
            return Result.Ok(await this._carDbContext.GetCarModels(brandName, name));
        }

        [HttpPost("Models")]
        public async Task<Result> AddCarModels(CarModel model)
        {
            CreateCarModelParamValidator validator = new CreateCarModelParamValidator(this._carDbContext);
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
            return Result.Ok(await this._carDbContext.GetCarSeries(brandName, series));
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
            return Result.Ok(await this._carDbContext.GetCarVersion(brandName, series));
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

        [HttpGet("Spec")]
        public async Task<Result> GetCarSpecInfo(CarSpecSearchParam param)
        {
            GetCarSpecParamValidator validator = new GetCarSpecParamValidator(this._carDbContext);
            ValidationResult vResult = validator.Validate(param);
            if (!vResult.IsValid)
            {
                return Result.Fail(vResult.Errors.First().ToString());
            }

            List<CarSpecList> carList = new List<CarSpecList>();
            List<CarVersion> versionList = this._carDbContext.WorldCarVersion.ToList();
            if (this._carDbContext.WorldCarPrefSpec != null)
            {
                if (param.speed != null)
                {
                    var carPrefSpeedList = this._carDbContext.WorldCarPrefSpec
                    .Where(x => x.SpecName.IndexOf("Top Speed") > -1)
                    .Where(x => param.speed >= this.GetSpecSpeed(x.SpecValue));

                    if (carPrefSpeedList?.Count() > 0)
                    {
                        versionList = versionList.Where(x => carPrefSpeedList.Any(y => y.VersionId.Equals(x.ID))).ToList();
                    }
                }

                if (param.acceleration != null)
                {
                    var carPrefAccelerationList = this._carDbContext.WorldCarPrefSpec
                    .Where(x => x.SpecName.IndexOf("Acceleration 0 to 100 km") > -1)
                    .Where(x => param.acceleration <= this.GetSpecAcceleration(x.SpecValue));

                    if (carPrefAccelerationList?.Count() > 0)
                    {
                        versionList = versionList.Where(x => carPrefAccelerationList.Any(y => y.VersionId.Equals(x.ID))).ToList();
                    }
                }
            }

            if (this._carDbContext.WorldCarEngineSpec != null)
            {
                if (param.engineSize != null)
                {
                    var carEngineSizeList = this._carDbContext.WorldCarEngineSpec
                        .Where(x => x.SpecName.IndexOf("Engine size") > -1)
                        .Where(x => param.engineSize >= this.GetSpecEngineSize(x.SpecValue));

                    if (carEngineSizeList?.Count() > 0)
                    {
                        versionList = versionList.Where(x => carEngineSizeList.Any(y => y.VersionId.Equals(x.ID))).ToList();
                    }
                }
            }

            if (this._carDbContext.WorldCarFuelSpec != null)
            {
                if (param.range != null)
                {
                    var carRangeList = this._carDbContext.WorldCarFuelSpec
                        .Where(x => x.SpecName.IndexOf("Range") > -1)
                        .Where(x => param.range <= this.GetSpecRange(x.SpecValue));

                    if (carRangeList?.Count() > 0)
                    {
                        versionList = versionList.Where(x => carRangeList.Any(y => y.VersionId.Equals(x.ID))).ToList();
                    }
                }
            }

            if (this._carDbContext.WorldCarSizeSpec != null)
            {
                if (param.doors != null)
                {
                    var carDoorsList = this._carDbContext.WorldCarSizeSpec
                        .Where(x => x.SpecName.IndexOf("Num. of Doors") > -1)
                        .Where(x => param.doors <= this.GetSpecDoors(x.SpecValue));

                    if (carDoorsList?.Count() > 0)
                    {
                        versionList = versionList.Where(x => carDoorsList.Any(y => y.VersionId.Equals(x.ID))).ToList();
                    }
                }

                if (param.seats != null)
                {
                    var carSeatsList = this._carDbContext.WorldCarSizeSpec
                        .Where(x => x.SpecName.IndexOf("Num. of Seats") > -1)
                        .Where(x => param.seats <= this.GetSpecSeats(x.SpecValue));

                    if (carSeatsList?.Count() > 0)
                    {
                        versionList = versionList.Where(x => carSeatsList.Any(y => y.VersionId.Equals(x.ID))).ToList();
                    }
                }
            }

            if (versionList?.Count() > 0)
            {
                foreach (var version in versionList)
                {
                    CarSpecList curCar = new CarSpecList();
                    curCar.Name = version.Name;
                    curCar.Engine = this._carDbContext.WorldCarEngineSpec.Where(x => x.VersionId.Equals(version.ID)).ToList();
                    curCar.Performance = this._carDbContext.WorldCarPrefSpec.Where(x => x.VersionId.Equals(version.ID)).ToList();
                    curCar.Size = this._carDbContext.WorldCarSizeSpec.Where(x => x.VersionId.Equals(version.ID)).ToList();
                    curCar.Fuel = this._carDbContext.WorldCarFuelSpec.Where(x => x.VersionId.Equals(version.ID)).ToList();
                    carList.Add(curCar);
                }
            }
            
            return Result.Ok(carList);
        }

        private int GetSpecSpeed(string specValue)
        {
            int speed = 0;
            Regex rg = new Regex(@$"(?<speed>\d+)\sKm");
            int.TryParse(rg.Matches(specValue).FirstOrDefault()?.Groups["speed"].Value, out speed);

            return speed;
        }

        private decimal GetSpecAcceleration(string specValue)
        {
            decimal acceleration = 0;
            Regex rg = new Regex(@"(?<acceleration>\d+\.\d+)\ss");
            decimal.TryParse(rg.Matches(specValue).FirstOrDefault()?.Groups["acceleration"].Value, out acceleration);

            return acceleration;
        }

        private int GetSpecEngineSize(string specValue)
        {
            int engineSize = 0;
            Regex rg = new Regex(@"(?<engineSize>\d+)\scm3");
            int.TryParse(rg.Matches(specValue).FirstOrDefault()?.Groups["engineSize"].Value, out engineSize);

            return engineSize;
        }

        private int GetSpecRange(string specValue)
        {
            int range = 0;
            Regex rg = new Regex(@"(?<range>\d+\.\d+)\ss");
            int.TryParse(rg.Matches(specValue).FirstOrDefault()?.Groups["range"].Value, out range);

            return range;
        }

        private int GetSpecDoors(string specValue)
        {
            int doors = 0;
            Regex rg = new Regex(@"(?<doors>\d+)");
            int.TryParse(rg.Matches(specValue).FirstOrDefault()?.Groups["doors"].Value, out doors);

            return doors;
        }

        private int GetSpecSeats(string specValue)
        {
            int seats = 0;
            Regex rg = new Regex(@"(?<seats>\d+)");
            int.TryParse(rg.Matches(specValue).FirstOrDefault()?.Groups["seats"].Value, out seats);

            return seats;
        }
    }
}