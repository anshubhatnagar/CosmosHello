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
    public static class GetProduct
    {
        [FunctionName("GetProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            string id = req.Query["id"];
            string name = req.Query["name"];

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
            {
                return new BadRequestObjectResult("Please pass 'id' and 'name' as query to this function.");
            }

            var dbContext = new DbContext(SettingsBuilder.BuildDbSettings());

            await dbContext.AddDatabaseWithContainerAsync();
            PlController product = await dbContext.GetItemAsync(id, name);

            return new OkObjectResult(product);
        }
    }
}
