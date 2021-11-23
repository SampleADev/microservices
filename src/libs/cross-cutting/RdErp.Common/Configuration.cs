using System;
using System.Linq;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RdErp
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddRdErpCommonServices(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            serviceCollection.AddSingleton<IClock, SystemClock>();

            return serviceCollection;
        }
    }
}