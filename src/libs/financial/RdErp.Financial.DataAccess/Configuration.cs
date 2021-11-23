using System;

using Microsoft.Extensions.DependencyInjection;

namespace RdErp.Financial
{
    public static class Configuration
    {
        public static IServiceCollection AddFinancialDataAccess(this IServiceCollection services)
        {
            return (services
                    ??
                    throw new ArgumentNullException(nameof(services)))
                .AddScoped<IFinancialTransactionDataAccessor, Linq2DbFinancialTransactionDataAccessor>();
        }
    }
}