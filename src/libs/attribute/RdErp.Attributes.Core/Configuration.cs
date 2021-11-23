using System;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace RdErp.Attributes
{
    public static class Configuration
    {
        public static IServiceCollection AddAttributeServices(this IServiceCollection services)
        {
            services = services
                ??
                throw new ArgumentNullException(nameof(services));

            return services
                .AddAttributesDataAccess()
                .AddScoped<ICustomAttributeService, CustomAttributeService>();
        }
    }
}