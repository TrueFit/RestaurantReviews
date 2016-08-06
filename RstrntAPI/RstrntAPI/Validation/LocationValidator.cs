using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using RstrntAPI.Models.Request;

namespace RstrntAPI.Validation
{
    internal class LocationValidator : AbstractValidator<LocationRequest>
    {
        public LocationValidator()
        {
            RuleSet("Create", () =>
            {
                RuleFor(x => x.Id).Null().WithMessage("Id cannot be specified");
                RuleFor(x => x.CityId).NotNull().WithMessage("CityId is required");
                RuleFor(x => x.RestaurantId).NotNull().WithMessage("RestaurantId is required");
                RuleFor(x => x.StreetAddress).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(0, 1024).WithMessage("StreetAddress must be <= 1024 characters");
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id).NotNull().WithMessage("Id required");
                RuleFor(x => x.CityId).NotNull().WithMessage("CityId is required");
                RuleFor(x => x.RestaurantId).NotNull().WithMessage("RestaurantId is required");
                RuleFor(x => x.StreetAddress).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(0, 1024).WithMessage("StreetAddress must be <= 1024 characters");
            });

            RuleSet("Lookup", () =>
            {
                RuleFor(x => x.Id).NotNull().WithMessage("Id required");
            });
        }
    }
}
