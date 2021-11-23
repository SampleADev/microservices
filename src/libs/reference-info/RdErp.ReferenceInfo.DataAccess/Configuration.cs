using System;

using Microsoft.Extensions.DependencyInjection;

using RdErp.ReferenceInfo.Currency;
using RdErp.ReferenceInfo.Financial;

namespace RdErp.ReferenceInfo
{
    public static class Configuration
    {
        public static IServiceCollection AddReferenceInfoDataAccess(this IServiceCollection services)
        {
            return (services
                    ??
                    throw new ArgumentNullException(nameof(services)))
                .AddScoped<IExchangeRateDataAccessor, Linq2DbExchangeRateDataAccessor>()
                .AddScoped<IFinancialRefDataAccessor, Linq2DbFinancialRefDataAccessor>();
        }
    }
}