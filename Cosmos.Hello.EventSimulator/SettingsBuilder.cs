using Cosmos.Hello.Entities;
using Microsoft.Azure.EventHubs;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cosmos.Hello.EventSimulator
{
    public static class SettingsBuilder
    {
        public static EventHubsConnectionStringBuilder BuildEventHubSettings()
        {
            var settings = new EventHubsConnectionStringBuilder(Environment.GetEnvironmentVariable("EventHubSettings:ConnectionString", EnvironmentVariableTarget.Process))
            {
                EntityPath = Environment.GetEnvironmentVariable("EventHubSettings:EventHubName")
            };

            return settings;
        }

        public static CosmosDbSettings BuildDbSettings()
        {
            var settings = new CosmosDbSettings
            {
                ConnectionString = Environment.GetEnvironmentVariable("CosmosDbSettings:ConnectionString", EnvironmentVariableTarget.Process),
                DatabaseName = Environment.GetEnvironmentVariable("CosmosDbSettings:DatabaseName", EnvironmentVariableTarget.Process),
                ContainerName = Environment.GetEnvironmentVariable("CosmosDbSettings:ContainerName", EnvironmentVariableTarget.Process)
            };

            return settings;
        }
    }
}
