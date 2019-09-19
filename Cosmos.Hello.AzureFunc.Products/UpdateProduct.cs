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

namespace Cosmos.Hello.AzureFunc.Products
{
    public static class UpdateProduct
    {
        [FunctionName("UpdateProduct")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            PlController product = JsonConvert.DeserializeObject<PlController>(requestBody);

            if (product == null)
            {
                return new BadRequestObjectResult("Please pass data of type 'Product' in the request body");
            }

            var dbContext = new DbContext(new DbSettings());

            await dbContext.AddDatabaseWithContainerAsync();
            await dbContext.UpdateItemAsync(product, product.Id, product.Name);

            log.LogInformation("Updated product to CosmosDB: {0}", product.Id);

            return new OkObjectResult("Command executed successfully: UpdateProduct");
        }
    }
}
