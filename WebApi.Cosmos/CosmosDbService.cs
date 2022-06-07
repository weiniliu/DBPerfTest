using Microsoft.Azure.Cosmos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Cosmos.Models;

namespace WebApi.Cosmos
{
    public class CosmosDbService : ICosmosDbService
    {
        private Container _container;

        public CosmosDbService(
            CosmosClient dbClient,
            string databaseName,
            string containerName)
        {
            this._container = dbClient.GetContainer(databaseName, containerName);
        }

        public async Task AddItemAsync(BotEventType item)
        {
            await this._container.CreateItemAsync<BotEventType>(item, new PartitionKey(item.BotId));
        }

        public async Task DeleteItemAsync(string id)
        {
            await this._container.DeleteItemAsync<BotEventType>(id, new PartitionKey(id));
        }

        public async Task<BotEventType> GetItemAsync(string id)
        {
            try
            {
                ItemResponse<BotEventType> response = await this._container.ReadItemAsync<BotEventType>(id, new PartitionKey(id));
                return response.Resource;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<List<BotEventType>> GetItemsAsync(string listOfIds)
        {
            try
            {
                string[] inputs = listOfIds.Split(",");
                IReadOnlyList<(string, PartitionKey)> itemList = new List<(string, PartitionKey)>();
                foreach (string input in inputs)
                {
                    itemList.Append((input, new PartitionKey(input)));
                }

                var response = await this._container.ReadManyItemsAsync<BotEventType>(itemList);
                return response.ToList();
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task<List<BotEventType>> GetAllItemsAsync()
        {
            try
            {
                List<BotEventType> res = new List<BotEventType>();
                string sqlQueryText = "SELECT * FROM c";
                QueryDefinition definition = new QueryDefinition(sqlQueryText);
                var iterator = _container.GetItemQueryIterator<BotEventType>(definition);
                while (iterator.HasMoreResults)
                {
                    FeedResponse<BotEventType> result = await iterator.ReadNextAsync();
                    foreach (BotEventType item in result)
                    {
                        res.Add(item);
                    }
                }
                return res;
            }
            catch (CosmosException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }
        }

        public async Task UpdateItemAsync(string id, BotEventType item)
        {
            await this._container.UpsertItemAsync<BotEventType>(item, new PartitionKey(id));
        }
    }
}
