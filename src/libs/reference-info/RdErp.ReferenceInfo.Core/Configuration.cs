using System;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using RdErp.ReferenceInfo.Currency;
using RdErp.ReferenceInfo.Financial;

namespace RdErp.ReferenceInfo
{
    public static class Configuration
    {
        public static IServiceCollection AddReferenceInfoServices(this IServiceCollection services)
        {
            services = services
                ??
                throw new ArgumentNullException(nameof(services));

            return services
                .AddReferenceInfoDataAccess()
                .AddScoped<IExchangeRateService, ExchangeRateService>()
                .AddScoped<IValidator<ExchangeRate>, ExchangeRateValidator>()
                .AddScoped<IExchangeRateSource, PrivatbankApiExchangeRateSource>()
                .AddScoped<ICurrencyListService, CurrencyListService>()
                .AddScoped<IFinancialRefService, FinancialRefService>();
        }
    }
}