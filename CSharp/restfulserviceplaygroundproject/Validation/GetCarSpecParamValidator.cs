using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FluentValidation;
using restfulserviceplaygroundproject.DatabaseContext;

namespace restfulserviceplaygroundproject.Validation
{
    public class GetCarSpecParamValidator : AbstractValidator<Model.CarSpecSearchParam>
    {
        CarsDbContext _carDbContext;
        public GetCarSpecParamValidator(CarsDbContext carsDbContext)
        {
            this._carDbContext = carsDbContext;
            When(param => {
                PropertyInfo[] properties = typeof(ParamArrayAttribute).GetProperties();
                return properties.All(x => null == x);
            }, () => {
                RuleFor(x => x.name).NotEmpty().WithMessage("Please use at least one param.");
            });

            When(param => param.speed != null, () => {
                RuleFor(x => x.speed).GreaterThan(0);
                RuleFor(x => x.speed).LessThan(1000);
            });

            When(param => param.engineSize != null, () => {
                RuleFor(x => x.engineSize).GreaterThan(0);
                RuleFor(x => x.engineSize).LessThan(5000);
            });

            When(param => param.range != null, () => {
                RuleFor(x => x.range).GreaterThan(0);
                RuleFor(x => x.range).LessThan(3000);
            });

            When(param => param.doors != null, () => {
                RuleFor(x => x.doors).GreaterThan(1);
                RuleFor(x => x.doors).LessThan(12);
            });

            When(param => param.seats != null, () => {
                RuleFor(x => x.seats).GreaterThan(1);
                RuleFor(x => x.seats).LessThan(60);
            });

            When(param => param.spec != null, () => {
                RuleFor(x => x.spec).NotEmpty();
            });

            When(param => param.specValue != null, () => {
                RuleFor(x => x.specValue).NotEmpty();
            });
        }
    }
}