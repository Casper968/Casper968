using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;

namespace restfulserviceplaygroundproject.Validation
{
    public class CreateCarBrandParamValidator : AbstractValidator<Model.CarBrand>
    {
        public CreateCarBrandParamValidator()
        {
            RuleFor(c => c.Name).NotEmpty();
            RuleFor(c => c.Url).NotEmpty();
        }
    }
}