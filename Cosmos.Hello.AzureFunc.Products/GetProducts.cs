using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Cosmos.Hello.Entities;
using System.Collections.Generic;

namespace Cosmos.Hello.AzureFunc.Products
{
    public static class GetProducts
    {
        [FunctionName("GetProducts")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            var dbContext = new DbContext(SettingsBuilder.BuildDbSettings());

            await dbContext.AddDatabaseWithContainerAsync();
            List<PlController> products = await dbContext.GetItemsAsync();

            log.LogInformation("Fetched all products from CosmosDB");

            return new OkObjectResult(products);
        }
    }
}
