using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace DotnetCosmosSDK
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await QueryforDocumentsAsync();
            await ViewDatabases();
            await CreateDatabase();
            await DeleteDatabase();
            await CreateDocument();
        }

        private static async Task CreateDocument()
        {
            var container = Shared.Client.GetContainer("adventure-works", "stores");
            dynamic document1Dynamic = new
            {
                addressId = "866",
                address = new
                {
                    addressLine1 = "bihar",
                    location = new
                    {
                        city = "Bothell"
                    },
                    postalCode = "98016"
                }
            };
            await container.CreateItemAsync(document1Dynamic, new PartitionKey("98016"));

    }

        private static async Task DeleteDatabase()
        {
            await Shared.Client.GetDatabase("MyNewDatabase").DeleteAsync();
        }

        private static async Task CreateDatabase()
        {
            var result = await Shared.Client.CreateDatabaseAsync("MyNewDatabase");
            var database = result.Resource;
            Console.WriteLine($"Database Id: {database.Id} ,Modified: {database.LastModified}");
        }

        private static async Task ViewDatabases()
        {
            var iterator = Shared.Client.GetDatabaseQueryIterator<DatabaseProperties>();
            var databases = await iterator.ReadNextAsync();
            var count = 0;
            foreach (var item in databases)
            {
                count++;
                Console.WriteLine($"Database Id: {item.Id} ,Modified: {item.LastModified}");
            }
            Console.WriteLine();
            Console.WriteLine($"No of databse: {count}");
        }

        private static async Task QueryforDocumentsAsync()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var endpoint = config["CosmosEndPoint"];
            var masterKey = config["CosmosMasterKey"];
            //create cosmos client using endpoint and masterkey
            using (var client = new CosmosClient(endpoint, masterKey))
            {
                // client.GetContainer(databasename, containername)
                var container = client.GetContainer("adventure-works", "stores");
                var sql = "SELECT * FROM c";
                var iterator = container.GetItemQueryIterator<dynamic>(sql);
                var page = await iterator.ReadNextAsync();
                foreach (var item in page)
                {
                    Console.WriteLine($"{item.id} {item.addressId}");
                }
                Console.ReadLine();
            }
        }
    }
}
