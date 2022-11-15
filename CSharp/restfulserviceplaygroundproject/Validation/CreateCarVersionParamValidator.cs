using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using restfulserviceplaygroundproject.DatabaseContext;

namespace restfulserviceplaygroundproject.Validation
{
    public class CreateCarVersionParamValidator : AbstractValidator<Model.CarVersion>
    {
        CarsDbContext _carDbContext;

        public CreateCarVersionParamValidator(CarsDbContext carsDbContext)
        {
            this._carDbContext = carsDbContext;
            When(series => this._carDbContext.WorldCarVersion.Any(x => x.Name.Equals(series.Name) && x.Url.Equals(series.Url)), () => {
                RuleFor(x => x.Name).Empty().WithMessage("Series data already exists.");
            });

            RuleFor(x => this._carDbContext.WorldCarSeries.First(y => y.ID.Equals(x.SeriesId))).NotEmpty().WithMessage("Cannot find series id from database.");
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Url).NotEmpty().WithMessage("Please provide series url.");
        }
    }
}