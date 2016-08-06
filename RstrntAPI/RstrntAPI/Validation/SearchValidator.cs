using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace RstrntAPI.Validation
{
    internal class SearchValidator : AbstractValidator<string>
    {
        public SearchValidator()
        {
            RuleFor(x => x).Cascade(CascadeMode.StopOnFirstFailure).NotNull().Length(2, 1024).WithMessage("AccountName must be between 2 and 1024 characters");
        }
    }
}
