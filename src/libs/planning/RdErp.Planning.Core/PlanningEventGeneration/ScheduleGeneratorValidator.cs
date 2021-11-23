using System;

using FluentValidation;

namespace RdErp.Planning
{
    public class ScheduleGeneratorValidator : AbstractValidator<Schedule>, IScheduleGeneratorValidator
    {
        public ScheduleGeneratorValidator()
        {
            RuleFor(e => e.ScheduleRule)
                .NotEmpty()
                .WithMessage("Schedule rule is required.");

            RuleFor(e => e.Currency)
                .NotEmpty()
                .WithMessage("Currency is required.");

            RuleFor(e => e.ValueExpression)
                .NotEmpty()
                .WithMessage("Expression is required.");
        }

    }
}