using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Cosmos.Hello.AzureFunc.Triggers
{
    public static class CosmosDbTrigger
    {
        [FunctionName("CosmosDbTrigger")]
        public static void Run([CosmosDBTrigger(
            databaseName: "AutomationDatabase",
            collectionName: "AutomationContainer",
            ConnectionStringSetting = "CosmosConnectionString",
            LeaseCollectionName = "leases",
            CreateLeaseCollectionIfNotExists = true)]IReadOnlyList<Document> input, ILogger log)
        {
            foreach (var item in input)
            {
                log.LogInformation("Document Modified: " + item.Id);
            }
        }

        public static string GetEnvironmentVariable(string name)
        {
            return name + ": " +
                Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
        }
    }
}
