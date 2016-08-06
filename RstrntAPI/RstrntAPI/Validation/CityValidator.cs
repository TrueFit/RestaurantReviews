using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using RstrntAPI.Models.Request;

namespace RstrntAPI.Validation
{
    internal class CityValidator : AbstractValidator<CityRequest>
    {
        public CityValidator()
        {
            RuleSet("Create", () =>
            {
                RuleFor(x => x.Id).Null().WithMessage("Id cannot be specified");
                RuleFor(x => x.Name).NotNull().WithMessage("Name is required");
                RuleFor(x => x.Name).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(0, 1024).WithMessage("Name must be <= 1024 characters");
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id).NotNull().WithMessage("Id required");
                RuleFor(x => x.Name).NotNull().WithMessage("Name required");
                RuleFor(x => x.Name).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(0, 1024).WithMessage("Name must be <= 1024 characters");
            });

            RuleSet("Lookup", () =>
            {
                RuleFor(x => x.Id).NotNull().WithMessage("Id required");
            });
        }
    }
}
