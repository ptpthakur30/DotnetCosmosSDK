using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetCosmosSDK
{
    public static class Shared
    {
        public static CosmosClient Client { get; private set; }
        static Shared()
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            var endpoint = config["CosmosEndPoint"];
            var masterKey = config["CosmosMasterKey"];
            Client = new CosmosClient(endpoint, masterKey);
        }
    }
}
