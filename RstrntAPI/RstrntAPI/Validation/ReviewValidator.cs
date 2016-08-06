using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using RstrntAPI.Models.Request;

namespace RstrntAPI.Validation
{
    internal class ReviewValidator : AbstractValidator<ReviewRequest>
    {
        public ReviewValidator()
        {
            RuleSet("Create", () =>
            {
                RuleFor(x => x.Id).Null().WithMessage("Id cannot be specified");
                RuleFor(x => x.Subject).NotNull().WithMessage("Subject is required");
                RuleFor(x => x.LocationId).NotNull().WithMessage("RestaurantId is required");
                RuleFor(x => x.UserId).NotNull().WithMessage("UserId is required");
                RuleFor(x => x.Subject).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(0, 1024).WithMessage("Subject must be <= 1024 characters");
                RuleFor(x => x.Body).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(0, 4096).WithMessage("Body must be <= 4096 characters");
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id).NotNull().WithMessage("Id required");
                RuleFor(x => x.Subject).NotNull().WithMessage("Subject is required");
                RuleFor(x => x.LocationId).NotNull().WithMessage("RestaurantId is required");
                RuleFor(x => x.UserId).NotNull().WithMessage("UserId is required");
                RuleFor(x => x.Subject).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(0, 1024).WithMessage("Subject must be <= 1024 characters");
                RuleFor(x => x.Body).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(0, 4096).WithMessage("Body must be <= 4096 characters");
            });

            RuleSet("Lookup", () =>
            {
                RuleFor(x => x.Id).NotNull().WithMessage("Id required");
            });
        }
    }
}
