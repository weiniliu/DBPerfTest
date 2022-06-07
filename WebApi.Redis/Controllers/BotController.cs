using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Cosmos;
using WebApi.Cosmos.Models;

namespace WebApi.Redis.Controllers
{
    public class BotController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;

        private readonly Cache _cache;

        public BotController(IConnectionMultiplexer multiplexer, ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
            _cache = new Cache(multiplexer);
        }

        [HttpGet("/bot/cache/{botId}")]
        public async Task<IActionResult> GetSubscriptionByIdAsync(string botId)
        {
            BotEventType bot = await _cache.Get<BotEventType>(botId);
            if (bot == null)
            {
                bot = await _cosmosDbService.GetItemAsync(botId);
                await _cache.Set(botId, bot, new DistributedCacheEntryOptions());
            }           
            return Ok(bot);
        }

        [HttpGet("/bots/cache/{listOfIds}")]
        public async Task<IActionResult> GetSubscriptionsByIdAsync(string listOfIds)
        {
            string[] items = listOfIds.Split(",");
            List<BotEventType> res = await _cache.GetMany<BotEventType>(listOfIds);
            if (res == null || res.Count < items.Length)
            {
                res = await _cosmosDbService.GetItemsAsync(listOfIds);
                foreach(BotEventType bot in res)
                {
                    await _cache.Set(bot.BotId, bot, new DistributedCacheEntryOptions());
                }
            }
            return Ok(res);
        }

        [HttpPost("/bots/cache")]
        public async Task<IActionResult> FillCacheAsync()
        {
            List<BotEventType> res = await _cosmosDbService.GetAllItemsAsync();
            foreach (BotEventType bot in res)
            {
                await _cache.Set(bot.BotId, bot, new DistributedCacheEntryOptions());
            }
            return Ok(res.Count);
        }
    }
}
