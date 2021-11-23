using System;

using FluentValidation;

namespace RdErp.ReferenceInfo.Currency
{
    public class ExchangeRateValidator : AbstractValidator<ExchangeRate>
        {
            public ExchangeRateValidator()
            {
                RuleFor(e => e.FromCurrency)
                    .Length(3).WithMessage("Source currency is not valid.");

                RuleFor(e => e.ToCurrency)
                    .Length(3).WithMessage("Target currency is not valid.");

                RuleFor(e => e.Rate)
                    .Must(v => v > 0).WithMessage("Exchange rate must be greater than zero");

            }
        }
}