using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using RstrntAPI.Models.Request;

namespace RstrntAPI.Validation
{
    internal class UserValidator : AbstractValidator<UserRequest>
    {
        public UserValidator()
        {
            RuleSet("Create", () =>
            {
                RuleFor(x => x.Id).Null().WithMessage("Id cannot be specified");
                RuleFor(x => x.AccountName).NotNull().WithMessage("AccountName is required");
                RuleFor(x => x.AccountName).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(0, 1024).WithMessage("AccountName must be <= 1024 characters");
                RuleFor(x => x.FullName).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(0, 1024).WithMessage("FullName must be <= 1024 characters");
            });

            RuleSet("Update", () =>
            {
                RuleFor(x => x.Id).NotNull().WithMessage("Id required");
                RuleFor(x => x.AccountName).NotNull().WithMessage("AccountName is required");
                RuleFor(x => x.AccountName).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(0, 1024).WithMessage("AccountName must be <= 1024 characters");
                RuleFor(x => x.FullName).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(0, 1024).WithMessage("FullName must be <= 1024 characters");
            });

            RuleSet("Lookup", () =>
            {
                RuleFor(x => x.Id).NotNull().WithMessage("Id required");
            });
        }
    }
}
