using System;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using RdErp.Reporting.FinancialResults;

namespace RdErp.Reporting
{
    public static class Configuration
    {
        public static IServiceCollection AddReportingServices(this IServiceCollection services)
        {
            services = services
                ??
                throw new ArgumentNullException(nameof(services));

            return services
                .AddReportingDataAccess()
                .AddScoped<IPlanningFinancialResultsReportService, PlanningFinancialResultsReportService>()
                .AddScoped<IMonthlyPlanFactResultsReportService, MonthlyPlanFactResultsReportService>();
        }
    }
}