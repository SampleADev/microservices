using System;

using FluentValidation;

namespace RdErp.Financial
{
    class FinancialTransactionAttributeValidator : AbstractValidator<FinancialTransactionAttribute>
        {
            public FinancialTransactionAttributeValidator()
            {
                RuleFor(m => m.Attribute)
                    .NotEmpty().WithMessage("Attribute is required.")
                    .Length(1, 20).WithMessage("Attribute name is too long.");

                RuleFor(m => m.Ref)
                    .NotEmpty()
                    .WithMessage("Reference is required.");
            }
        }
}