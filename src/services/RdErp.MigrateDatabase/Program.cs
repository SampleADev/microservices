using System;
using System.Collections.Generic;

using Npgsql;

namespace RdErp.MigrateDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Migrating RD-ERP Database");
            var configuration = ConfigurationProvider.Provide();
            var connectionString = configuration["Evolve:ConnectionString"];
            var location = configuration["Evolve:Locations"];

            Console.WriteLine($"Database: {connectionString}");
            Console.WriteLine($"Migrations location: {location}");

            try
            {
                using(var cnx = new NpgsqlConnection(connectionString))
                {
                    cnx.Open();
                    var evolve = new Evolve.Evolve(cnx, msg => Console.WriteLine(msg))
                    {
                        Locations = new List<string> { location },
                        IsEraseDisabled = false,
                    };

                    evolve.Migrate();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Database migration failed.", ex);
                throw;
            }
        }
    }
}