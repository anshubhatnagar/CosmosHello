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

namespace Cosmos.Hello.Serverless
{
    public static class DeleteProduct
    {
        [FunctionName("DeleteProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req,
            ILogger log)
        {
            string id = req.Query["id"];
            string name = req.Query["name"];

            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
            {
                return new BadRequestObjectResult("Please pass 'id' and 'name' as query to this function.");
            }

            var dbContext = new DbContext(new DbSettings());

            await dbContext.AddDatabaseWithContainerAsync();
            await dbContext.DeleteItemAsync(id, name);

            log.LogInformation("Deleted product to CosmosDB: {0}", id);

            return new OkObjectResult("Command executed successfully: DeleteProduct");
        }
    }
}
