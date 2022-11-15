using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using restfulserviceplaygroundproject.DatabaseContext;

namespace restfulserviceplaygroundproject.Validation
{
    public class CreateCarModelParamValidator  : AbstractValidator<Model.CarModel>
    {
        CarsDbContext _carDbContext;

        public CreateCarModelParamValidator(CarsDbContext carsDbContext)
        {
            this._carDbContext = carsDbContext;
            RuleFor(x => this._carDbContext.WorldCarBrand.First(y => y.ID.Equals(x.BrandId))).NotEmpty().WithMessage("Cannot find brand id from database.");
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.ListingYear).GreaterThan(1850).WithMessage("Listing year should greater than 1850");
            RuleFor(x => x.Url).NotEmpty().WithMessage("Please provide model url.");
        }
    }
}