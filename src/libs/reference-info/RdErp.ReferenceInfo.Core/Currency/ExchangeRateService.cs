using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

namespace RdErp.ReferenceInfo.Currency
{
    public class ExchangeRateService : IExchangeRateService
    {
        private readonly IEnumerable<IExchangeRateSource> exchangeRateSources;
        private readonly IExchangeRateDataAccessor exchangeRateDataAccessor;
        private readonly IValidator<ExchangeRate> exchangeRateValidator;

        public ExchangeRateService(
            IEnumerable<IExchangeRateSource> exchangeRateSources,
            IExchangeRateDataAccessor exchangeRateDataAccessor,
            IValidator<ExchangeRate> exchangeRateValidator
        )
        {
            this.exchangeRateSources = exchangeRateSources
                ??
                throw new ArgumentNullException(nameof(exchangeRateSources));
            this.exchangeRateDataAccessor = exchangeRateDataAccessor
                ??
                throw new ArgumentNullException(nameof(exchangeRateDataAccessor));
            this.exchangeRateValidator = exchangeRateValidator
                ??
                throw new ArgumentNullException(nameof(exchangeRateValidator));
        }

        public async Task RefreshExchangeRates(string rateSource)
        {
            var source = exchangeRateSources
                .FirstOrDefault(r => !String.IsNullOrWhiteSpace(rateSource)
                    && StringComparer.OrdinalIgnoreCase.Equals(rateSource, r.Name))
                ?? exchangeRateSources.FirstOrDefault(r => r.IsDefault);

            if (source == null)
            {
                throw new InvalidOperationException("Can't find a valid source to get exchange rates from");
            }

            var rates = (await source.LoadExchangeRates(DateTime.Now.AddYears(-1), DateTime.Now))
                .ToArray();

            await exchangeRateValidator.ValidateExtAsync(rates);

            exchangeRateDataAccessor.Insert(rates);
        }
    }
}