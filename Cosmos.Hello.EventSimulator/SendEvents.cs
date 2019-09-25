using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Azure.EventHubs;
using Cosmos.Hello.Entities;
using System.Text;

namespace Cosmos.Hello.EventSimulator
{
    public static class SendEvents
    {
        [FunctionName("SendEvents")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            if (JsonConvert.DeserializeObject<PlController>(requestBody) == null)
            {
                return new BadRequestObjectResult("Please pass data of type 'Product' in the request body");
            }

            // Creates an EventHubsConnectionStringBuilder object from the connection string, and sets the EntityPath.
            // Typically, the connection string should have the entity path in it, but this simple scenario
            // uses the connection string from the namespace.
            var connectionStringBuilder = SettingsBuilder.BuildEventHubSettings();

            EventHubClient eventHubClient = EventHubClient.CreateFromConnectionString(connectionStringBuilder.ToString());

            await SendMessagesToEventHub(requestBody, eventHubClient, 20, log);
            await eventHubClient.CloseAsync();

            return new OkObjectResult("Command executed successfully: Product message sent to EventHub");
        }

        private static async Task SendMessagesToEventHub(string productJson, EventHubClient eventHubClient, int numMessagesToSend, ILogger log)
        {
            for (var i = 0; i < numMessagesToSend; i++)
            {
                try
                {
                    var productCopy = JsonConvert.DeserializeObject<PlController>(productJson);
                    Random random = new Random();

                    productCopy.Id = Guid.NewGuid().ToString();
                    productCopy.Name = "Event Controller " + random.Next(int.MaxValue);
                    productCopy.MaxCapacityInKilobytes = random.Next(2048);

                    var productCopyJson = JsonConvert.SerializeObject(productCopy);

                    await eventHubClient.SendAsync(new EventData(Encoding.UTF8.GetBytes(productCopyJson)));
                }
                catch (Exception ex)
                {
                    log.LogInformation(ex.Message);
                }
            }
        }
    }
}
