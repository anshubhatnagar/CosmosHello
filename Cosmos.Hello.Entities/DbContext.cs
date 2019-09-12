using Microsoft.Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace Cosmos.Hello.Entities
{
    public class DbContext
    {
        private DbSettings _dbSettings;
        private CosmosClient _client;
        private Database _database;
        private Container _container;

        public DbContext(DbSettings dbSettings)
        {
            _dbSettings = dbSettings;
            _client = new CosmosClient(_dbSettings.EndpointUri, _dbSettings.PrimaryKey);
        }

        public async Task AddDatabaseWithContainerAsync()
        {
            _database = await _client.CreateDatabaseIfNotExistsAsync(_dbSettings.DatabaseId);

            // Has Pricing Implications
            _container = await _database.CreateContainerIfNotExistsAsync(_dbSettings.ContainerId, "/Name");
        }

        public async Task AddItemsToContainerAsync()
        {
            var controller = new PlController
            {
                Id = "MicroSmart.1",
                Name = "MicroSmart Pentra",
                Family = "Idec MicroSmart",
                MaxDigital = 512,
                MaxCapacityInKilobytes = 128,
                NetworkInterfaces = new string[]
                {
                    "Modbus",
                    "AS-interface"
                }
            };

            ItemResponse<PlController> response = await _container.CreateItemAsync<PlController>(controller, new PartitionKey(controller.Name));

            controller = new PlController
            {
                Id = "OpenNet Controller.1",
                Name = "OpenNet Controller",
                Family = "OpenNet Controller",
                MaxDigital = 480,
                MaxCapacityInKilobytes = 32,
                NetworkInterfaces = new string[]
                {
                    "Interbus"
                }
            };

            response = await _container.CreateItemAsync<PlController>(controller, new PartitionKey(controller.Name));
        }

        public async Task<List<PlController>> GetItemsAsync()
        {
            var sqlQueryText = "SELECT * FROM c";

            var queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<PlController> queryResultSetIterator = _container.GetItemQueryIterator<PlController>(queryDefinition);
            var controllers = new List<PlController>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<PlController> currentResultSet = await queryResultSetIterator.ReadNextAsync();

                foreach (PlController controller in currentResultSet)
                {
                    controllers.Add(controller);
                }
            }

            return controllers;
        }

        public async Task<PlController> UpdateItemAsync(PlController controller, string id, string partitionKey)
        {
            var response = await _container.ReplaceItemAsync<PlController>(controller, controller.Id, new PartitionKey(controller.Name));
            return response.Resource;
        }

        public async Task DeleteItemAsync(string id, string partitionKey)
        {
            await _container.DeleteItemAsync<PlController>(id, new PartitionKey(partitionKey));
        }

        public async Task DeleteDatabaseAsync()
        {
            await _database.DeleteAsync();
        }
    }
}
