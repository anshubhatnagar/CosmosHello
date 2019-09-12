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
        private Container _container;

        public DbContext(DbSettings dbSettings)
        {
            _dbSettings = dbSettings;
        }

        public async Task OpenConnectionAsync()
        {
            var client = new CosmosClient(_dbSettings.EndpointUri, _dbSettings.PrimaryKey);
            var databaseResponse = await client.CreateDatabaseIfNotExistsAsync(_dbSettings.DatabaseId);

            // Has Pricing Implications
            _container = await databaseResponse.Database.CreateContainerIfNotExistsAsync(_dbSettings.ContainerId, "/Name");
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

            try
            {
                ItemResponse<PlController> response = await _container.ReadItemAsync<PlController>(controller.Id, new PartitionKey(controller.Name));
                Console.WriteLine("Already Exists: {0}", response.Resource.Id);
            }
            catch (CosmosException cosmosEx) when (cosmosEx.StatusCode == HttpStatusCode.NotFound)
            {
                ItemResponse<PlController> response = await _container.CreateItemAsync<PlController>(controller, new PartitionKey(controller.Name));
                Console.WriteLine("Created: {0}", response.Resource.Id);
            }

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

            try
            {
                ItemResponse<PlController> response = await _container.ReadItemAsync<PlController>(controller.Id, new PartitionKey(controller.Name));
                Console.WriteLine("Already Exists: {0}", response.Resource.Id);
            }
            catch (CosmosException cosmosEx) when (cosmosEx.StatusCode == HttpStatusCode.NotFound)
            {
                ItemResponse<PlController> response = await _container.CreateItemAsync<PlController>(controller, new PartitionKey(controller.Name));
                Console.WriteLine("Created: {0}", response.Resource.Id);
            }
        }

        public async Task ReadItemsQueryAsync()
        {
            var sqlQueryText = "SELECT * FROM c WHERE c.Name = 'MicroSmart Pentra'";

            Console.WriteLine("Running query: {0}\n", sqlQueryText);

            var queryDefinition = new QueryDefinition(sqlQueryText);
            FeedIterator<PlController> queryResultSetIterator = _container.GetItemQueryIterator<PlController>(queryDefinition);

            var families = new List<PlController>();

            while (queryResultSetIterator.HasMoreResults)
            {
                FeedResponse<PlController> currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (PlController controller in currentResultSet)
                {
                    families.Add(controller);
                    Console.WriteLine("\tRead {0}\n", controller);
                }
            }
        }

        public async Task ReplaceItemAsync()
        {
            ItemResponse<PlController> wakefieldFamilyResponse = await _container.ReadItemAsync<PlController>("OpenNet Controller.1", new PartitionKey("OpenNet Controller"));
            var controller = wakefieldFamilyResponse.Resource;

            controller.MaxDigital = 512;

            // replace the item with the updated content
            wakefieldFamilyResponse = await _container.ReplaceItemAsync<PlController>(controller, controller.Id, new PartitionKey(controller.Name));
            Console.WriteLine("Updated Family [{0},{1}].\n \tBody is now: {2}\n", controller.Name, controller.Id, wakefieldFamilyResponse.Resource);
        }

        public async Task DeleteItemAsync()
        {
            var partitionKeyValue = "OpenNet Controller";
            var id = "OpenNet Controller.1";

            // Delete an item. Note we must provide the partition key value and id of the item to delete
            ItemResponse<PlController> wakefieldFamilyResponse = await _container.DeleteItemAsync<PlController>(id, new PartitionKey(partitionKeyValue));
            Console.WriteLine("Deleted Family [{0},{1}]\n", partitionKeyValue, id);
        }
    }
}
