using System;

using Microsoft.Extensions.DependencyInjection;

namespace RdErp.Planning
{
    public static class Configuration
    {
        public static IServiceCollection AddPlanningDataAccess(this IServiceCollection services)
        {
            return (services
                    ??
                    throw new ArgumentNullException(nameof(services)))
                .AddScoped<IPlanDataAccessor, Linq2DbPlanDataAccessor>()
                .AddScoped<IScheduleDataAccessor, Linq2DbScheduleDataAccessor>()
                .AddScoped<IPlanningEventDataAccessor, Linq2DbPlanningEventDataAccessor>()
                .AddScoped<IScheduleAttributeDataAccessor, Linq2DbScheduleAttributeDataAccessor>();
        }
    }
}