using System;

using FluentValidation;

namespace RdErp.Planning
{
    public class ScheduleDetailsValidator : AbstractValidator<ScheduleDetails>, IValidator<ScheduleDetails>
        {
            public ScheduleDetailsValidator(
                IValidator<ScheduleAttribute> scheduleAttributeValidator
            ) : base()
            {
                if (scheduleAttributeValidator == null)
                {
                    throw new ArgumentNullException(nameof(scheduleAttributeValidator));
                }

                this.Inherit(new ScheduleGeneratorValidator());

                RuleFor(m => m.Currency)
                    .NotEmpty().WithMessage("Currency is required.")
                    .Length(1, 3).WithMessage("Currency too long.");

                RuleFor(e => e.Name)
                    .NotEmpty().WithMessage("Name is required.")
                    .Length(1, 512).WithMessage("Name is too long.");

                RuleFor(e => e.StartDate)
                    .Must(d => d != DateTimeOffset.MinValue)
                    .WithMessage("Start date is required.");

                RuleFor(e => e.EndDate)
                    .Must((entity, value) => !value.HasValue || value.Value > entity.StartDate)
                    .WithMessage("End date must be greater than start date.");

                RuleFor(e => e.Attributes)
                    .SetCollectionValidator(scheduleAttributeValidator);
            }

        }
}