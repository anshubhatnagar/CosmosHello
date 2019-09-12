using Cosmos.Hello.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cosmos.Hello.ConsoleClient
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var dbSettings = new DbSettings
            {
                EndpointUri = "",
                PrimaryKey = "",
                DatabaseId = "",
                ContainerId = ""
            };

            var dbContext = new DbContext(dbSettings);
            await dbContext.OpenConnectionAsync();
            await dbContext.AddItemsToContainerAsync();
            await dbContext.ReadItemsQueryAsync();
            await dbContext.ReplaceItemAsync();
            await dbContext.DeleteItemAsync();
            Console.WriteLine("Done.");
            Console.ReadLine();
        }
    }
}
