using System;

using FluentValidation;

namespace RdErp.Planning
{
    public class ScheduleAttributeValidator : AbstractValidator<ScheduleAttribute>
        {
            public ScheduleAttributeValidator()
            {
                RuleFor(e => e.Attribute)
                    .NotEmpty().WithMessage("Attribute is required")
                    .Length(1, 20).WithMessage("Attribute is too long");

                RuleFor(e => e.Ref)
                    .NotEmpty().WithMessage("Value is required");
            }
        }
}