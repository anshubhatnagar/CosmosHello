using Cosmos.Hello.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cosmos.Hello.AzureFunc.Products
{
    public static class SettingsBuilder
    {
        public static CosmosDbSettings BuildDbSettings()
        {
            var settingFilePath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            var builder = new ConfigurationBuilder()
                          .AddJsonFile(settingFilePath);

            var configuration = builder.Build();

            var settings = new CosmosDbSettings
            {
                ConnectionString = configuration["CosmosDbSettings:ConnectionString"],
                DatabaseName = configuration["CosmosDbSettings:DatabaseName"],
                ContainerName = configuration["CosmosDbSettings:ContainerName"]
            };

            return settings;
        }
    }
}
