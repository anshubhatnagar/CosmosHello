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

namespace Cosmos.Hello.EventSimulator
{
    public static class StoreEvents
    {
        [FunctionName("StoreEvents")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            List<PlController> itemList = JsonConvert.DeserializeObject<List<PlController>>(requestBody);

            if (itemList == null)
            {
                return new BadRequestObjectResult("Please pass array of objects in the request body");
            }

            var dbContext = new DbContext(SettingsBuilder.BuildDbSettings());

            await dbContext.AddDatabaseWithContainerAsync();

            foreach (var item in itemList)
            {
                await dbContext.AddItemToContainerAsync(item);
            }
            
            return new OkObjectResult("Command executed successfully: StoreEvents");
        }
    }
}
