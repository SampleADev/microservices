using System;

using FluentValidation;

namespace RdErp.Financial
{
    class FinancialTransactionValidator : AbstractValidator<FinancialTransaction>
        {
            public FinancialTransactionValidator()
            {
                RuleFor(m => m.Amount)
                    .NotEmpty().WithMessage("Amount should not be zero.");

                RuleFor(m => m.Code)
                    .NotEmpty().WithMessage("Transaction code is required.");

                RuleFor(m => m.CostCenterId)
                    .NotEmpty().WithMessage("Cost center is required.");

                RuleFor(m => m.Currency)
                    .NotEmpty().WithMessage("Currency is required.");

                RuleFor(m => m.ReferenceType)
                    .NotEmpty().When(m =>(m.ReferenceId??0) != 0)
                    .WithMessage("Reference type is required if reference ID is specified.");

                RuleFor(m => m.ReferenceId)
                    .NotEmpty().When(m => !String.IsNullOrWhiteSpace(m.ReferenceType))
                    .WithErrorCode("Reference ID is required if reference type is specified.");

            }
        }
}