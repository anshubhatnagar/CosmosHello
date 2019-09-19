using Cosmos.Hello.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Hello.ConsoleClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("Establishing Connection...");
            var dbContext = new DbContext(new CosmosDbSettings());
            await dbContext.AddDatabaseWithContainerAsync();

            Console.WriteLine("Reading Items...");
            var items = await dbContext.GetItemsAsync();

            if (items.Count > 0)
            {
                Console.WriteLine("{0} Items Found. Recreating Database...", items.Count);
                await dbContext.DeleteDatabaseAsync();
                await dbContext.AddDatabaseWithContainerAsync();
                await dbContext.AddMockItemsToContainerAsync();
            }

            Console.WriteLine("Reading Items...");
            items = await dbContext.GetItemsAsync();

            Console.WriteLine("{0} Items Found. Updating last item...", items.Count);

            PlController lastController = items.Last();
            lastController.MaxDigital++;

            await dbContext.UpdateItemAsync(lastController, lastController.Id, lastController.Name);

            Console.WriteLine("Deleting last item...");
            await dbContext.DeleteItemAsync(lastController.Id, lastController.Name);

            Console.WriteLine("Cosmos Demo Complete. Press any key to exit...");
            Console.ReadLine();
        }
    }
}
