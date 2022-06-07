using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Cosmos.Models;

namespace WebApi.Cosmos.Controllers
{
    public class BotController : Controller
    {
        private readonly ICosmosDbService _cosmosDbService;
        public BotController(ICosmosDbService cosmosDbService)
        {
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet("/bot/{botId}")]
        public async Task<IActionResult> GetSubscriptionByIdAsync(string botId)
        {
            BotEventType res = await _cosmosDbService.GetItemAsync(botId);
            return Ok(res);
        }

        [HttpGet("/bots/{listOfIds}")]
        public async Task<IActionResult> GetSubscriptionsByIdAsync(string listOfIds)
        {
            List<BotEventType> res = await _cosmosDbService.GetItemsAsync(listOfIds);
            return Ok(res);
        }

        [HttpPost("/bots")]
        public async Task<IActionResult> AddMoreBotsAsync()
        {
            for (int i = 0; i < 1000; i++)
            {
                var id = Guid.NewGuid();
                await _cosmosDbService.AddItemAsync(new BotEventType
                {
                    BotId = id.ToString(),
                    EventTypes = new List<string>() { "Microsoft.Communication.CallParticipantAdded", "Microsoft.Communication.CallParticipantRemoved" }
                });
            }
            return Ok();
        }

        [HttpGet]
        [Route("loaderio-827fc2106366cb4f5400c1f53e8082c3")]
        public IActionResult GetLoaderIoVerification()
        {
            byte[] fileBytes = System.IO.File.ReadAllBytes("C:/DBPerfTest/loaderio-827fc2106366cb4f5400c1f53e8082c3.txt");
            return File(fileBytes, "application/x-msdownload", "loaderio-827fc2106366cb4f5400c1f53e8082c3.txt");
        }

    }
}
