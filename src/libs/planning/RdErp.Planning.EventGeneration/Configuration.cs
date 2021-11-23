using System;

using Microsoft.Extensions.DependencyInjection;

namespace RdErp.Planning.EventGeneration
{
    public static class Configuration
    {
        public static IServiceCollection AddEventGenerators(this IServiceCollection services)
        {
            services = services
                ??
                throw new ArgumentNullException(nameof(services));

            return services
                .AddSingleton<IEventDateGenerator, EachDaysOfMonthEventDateGenerator>()
                .AddSingleton<IEventDateGenerator, EachDaysOfWeekEventDateGenerator>()
                .AddSingleton<IEventDateGenerator, EachNumOfDaysEventDateGenerator>()
                .AddSingleton<IEventDateGenerator, StaticDatesEventDateGenerator>();
        }
    }
}