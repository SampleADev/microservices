using System;

using FluentValidation;

namespace RdErp.Planning
{
    public class PlanValidator : AbstractValidator<Plan>
        {
            public PlanValidator()
            {
                RuleFor(r => r.Name)
                    .NotEmpty().WithMessage("Name is required")
                    .Length(0, 512).WithMessage("Name is too long");
            }
        }
}