using System;

using Microsoft.Extensions.DependencyInjection;

namespace RdErp.Attributes
{
    public static class Configuration
    {
        public static IServiceCollection AddAttributesDataAccess(this IServiceCollection services)
        {
            return (services
                    ??
                    throw new ArgumentNullException(nameof(services)))
                .AddScoped<ICustomAttributeDataAccessor, CustomerAttributeDataAccessor>()
                .AddScoped<ICustomAttributeDataAccessor, ProjectAttributeDataAccessor>()
                .AddScoped<ICustomAttributeDataAccessor, EmployeeAttributeDataAccessor>();
        }
    }
}