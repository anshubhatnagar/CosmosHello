using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Hello.Entities
{
    public class DbSettings
    {
        // The Azure Cosmos DB endpoint for running this sample.
        public string EndpointUri { get; set; }

        // The primary key for the Azure Cosmos account.
        public string PrimaryKey { get; set; }

        // The name of the database and container we will create
        public string DatabaseId { get; set; }
        public string ContainerId { get; set; }

        public DbSettings()
        {
            EndpointUri = "";
            PrimaryKey = "";
            DatabaseId = "";
            ContainerId = "";
        }
    }
}
