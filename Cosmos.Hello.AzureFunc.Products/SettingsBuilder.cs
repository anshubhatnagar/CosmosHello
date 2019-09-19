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
        public static CosmosDbSettings BuildDbSettings(string baseDirectory)
        {
            var config = new ConfigurationBuilder()
                          .SetBasePath(baseDirectory)
                          .AddJsonFile("appsettings.json")
                          .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                          .Build();

            var settings = new CosmosDbSettings
            {
                ConnectionString = config["CosmosDbSettings:ConnectionString"],
                DatabaseName = config["CosmosDbSettings:DatabaseName"],
                ContainerName = config["CosmosDbSettings:ContainerName"]
            };

            return settings;
        }
    }
}
