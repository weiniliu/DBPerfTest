using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Cosmos.Models;

namespace WebApi.Cosmos
{
    public interface ICosmosDbService
    {
        Task<List<BotEventType>> GetItemsAsync(string listOfIds);
        Task<BotEventType> GetItemAsync(string id);
        Task AddItemAsync(BotEventType item);
        Task UpdateItemAsync(string id, BotEventType item);
        Task DeleteItemAsync(string id);
        Task<List<BotEventType>> GetAllItemsAsync();
    }
}
