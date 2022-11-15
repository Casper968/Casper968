using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using restfulserviceplaygroundproject.DatabaseContext;

namespace restfulserviceplaygroundproject.Validation
{
    public class CreateCarSeriesParamValidator : AbstractValidator<Model.CarSeries>
    {
        CarsDbContext _carDbContext;

        public CreateCarSeriesParamValidator(CarsDbContext carsDbContext)
        {
            this._carDbContext = carsDbContext;
            When(series => this._carDbContext.WorldCarSeries.Any(x => x.Name.Equals(series.Name) && x.Url.Equals(series.Url)), () => {
                RuleFor(x => x.Name).Empty().WithMessage("Series data already exists.");
            });

            RuleFor(x => this._carDbContext.WorldCarModel.First(y => y.ID.Equals(x.ModelId))).NotEmpty().WithMessage("Cannot find model id from database.");
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Url).NotEmpty().WithMessage("Please provide series url.");
        }
    }
}