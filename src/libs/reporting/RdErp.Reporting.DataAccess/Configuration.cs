using System;

using Microsoft.Extensions.DependencyInjection;

using RdErp.Reporting.FinancialResults;

namespace RdErp.Reporting
{
    public static class Configuration
    {
        public static IServiceCollection AddReportingDataAccess(this IServiceCollection services) =>
            (services
                ??
                throw new ArgumentNullException(nameof(services)))
            .AddScoped<IPlanningFinancialResultsReportDataAccessor, Linq2DbPlanningFinancialResultsReportDataAccessor>()
            .AddScoped<IMonthlyPlanFactResultsReportDataAccessor, Linq2DbMonthlyPlanFactResultsReportDataAccessor>();
    }
}