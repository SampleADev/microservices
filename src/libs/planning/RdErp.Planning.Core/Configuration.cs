using System;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

using RdErp.Planning.EventGeneration;

namespace RdErp.Planning
{
    public static class Configuration
    {
        public static IServiceCollection AddPlanningServices(this IServiceCollection services)
        {
            services = services
                ??
                throw new ArgumentNullException(nameof(services));

            return services
                .AddPlanningDataAccess()
                // Event generation
                .AddEventGenerators()
                .AddScoped<IScheduleGeneratorValidator, ScheduleGeneratorValidator>()
                .AddScoped<IPlanningEventGenerationService, PlanningEventGenerationService>()
                .AddScoped<IValueExpressionEvaluatorFactory, DynamicExpressoEvaluatorFactory>()
                // Plan management
                .AddScoped<IValidator<Plan>, PlanValidator>()
                .AddScoped<IPlanManagementService, PlanManagementService>()
                // Schedule management
                .AddScoped<IValidator<ScheduleDetails>, ScheduleDetailsValidator>()
                .AddScoped<IValidator<ScheduleAttribute>, ScheduleAttributeValidator>()
                .AddScoped<IScheduleManagementService, ScheduleManagementService>()
                // Planning event management
                .AddScoped<IPlanningEventManagementService, PlanningEventManagementService>();
        }
    }
}