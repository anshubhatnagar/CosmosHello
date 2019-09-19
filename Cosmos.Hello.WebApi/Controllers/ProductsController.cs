using Cosmos.Hello.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Cosmos.Hello.WebApi.Controllers
{
    public class ProductsController : ApiController
    {
        private DbContext _dbContext;

        public ProductsController()
        {
            CosmosDbSettings settings = BuildDbSettings();

            _dbContext = new DbContext(settings);
        }

        [HttpGet]
        public async Task<IEnumerable<PlController>> GetAll()
        {
            await _dbContext.AddDatabaseWithContainerAsync();
            return await _dbContext.GetItemsAsync();
        }

        [HttpGet]
        public async Task<PlController> Get(string id, string name)
        {
            await _dbContext.AddDatabaseWithContainerAsync();
            return await _dbContext.GetItemAsync(id, name);
        }

        [HttpPost]
        public async Task Update([FromBody]PlController value)
        {
            await _dbContext.AddDatabaseWithContainerAsync();
            await _dbContext.UpdateItemAsync(value, value.Id, value.Name);
        }

        [HttpPut]
        public async Task Insert([FromBody]PlController value)
        {
            await _dbContext.AddDatabaseWithContainerAsync();
            await _dbContext.AddItemToContainerAsync(value);
        }

        [HttpDelete]
        public async Task Delete(string id, string name)
        {
            await _dbContext.AddDatabaseWithContainerAsync();
            await _dbContext.DeleteItemAsync(id, name);
        }

        private CosmosDbSettings BuildDbSettings()
        {
            var builder = new ConfigurationBuilder()
                          .AddJsonFile("appsettings.json");

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