using System;
using System.IO;

using Microsoft.Extensions.Configuration;

namespace RdErp.MigrateDatabase
{
    public static class ConfigurationProvider
    {
        private const string CONFIG_FILE_NAME = "appsettings.json";

        public static IConfiguration Provide()
        {

            var builder = new ConfigurationBuilder()
                .AddJsonFileIfExists(String.Join(Path.DirectorySeparatorChar.ToString(), "..", "..", CONFIG_FILE_NAME))
                .AddJsonFileIfExists(String.Join(Path.DirectorySeparatorChar.ToString(), "..", CONFIG_FILE_NAME))
                .AddJsonFileIfExists(String.Join(Path.DirectorySeparatorChar.ToString(), ".", CONFIG_FILE_NAME))
                .AddEnvironmentVariables();

            return builder.Build();
        }

        private static IConfigurationBuilder AddJsonFileIfExists(this IConfigurationBuilder builder, string path)
        {
            var absolutePath = AbsolutePath(path);
            if (File.Exists(absolutePath))
            {
                return builder.AddJsonFile(absolutePath);
            }

            return builder;
        }

        private static string AbsolutePath(string relativePath)
        {
            return Path.Combine(Environment.CurrentDirectory, relativePath);
        }
    }

}