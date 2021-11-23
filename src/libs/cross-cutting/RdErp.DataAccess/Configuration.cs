using System;
using System.Linq;

using LinqToDB.Configuration;
using LinqToDB.Data;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RdErp.DataAccess
{
    public static class ConfigurationExtensions
    {
        public static IServiceCollection AddRdErpDataAccess(this IServiceCollection serviceCollection)
        {
            if (serviceCollection == null)
            {
                throw new ArgumentNullException(nameof(serviceCollection));
            }

            DataConnection.TurnTraceSwitchOn();
            DataConnection.WriteTraceLine = (message, displayName) =>
                Console.WriteLine($"{message} {displayName}");

            serviceCollection
                .AddScoped<DataConnection>(svc =>
                {
                    var settings = svc.GetService<IConfiguration>();

                    var dc = new DataConnection(
                        settings.GetValue<string>("Db:ProviderName"),
                        settings.GetValue<string>("Db:ConnectionString")
                    );

                    return dc;
                })
                .AddScoped<ITransactionManager, Linq2DbTransactionManager>();

            return serviceCollection;
        }
    }
}