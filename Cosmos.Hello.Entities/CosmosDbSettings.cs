using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Hello.Entities
{
    public class CosmosDbSettings
    {
        // The Azure Cosmos DB endpoint for running this sample.
        public string ConnectionString { get; set; }

        // The name of the database and container we will create
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }

    }
}
