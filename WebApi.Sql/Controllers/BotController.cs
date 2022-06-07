using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Sql.Models;

namespace WebApi.Sql.Controllers
{
    public class BotController : Controller
    {
        private readonly BotSubscriptionContext _context;

        public BotController(BotSubscriptionContext context)
        {
            _context = context;
        }

        [HttpGet("/bot/{botId}")]
        public async Task<IActionResult> GetSubscriptionByIdAsync(string botId)
        {
            Guid id = Guid.Parse(botId);
            List<BotEventType> res = await _context.Subscriptions.Where(s => s.BotId == botId).ToListAsync();
            return Ok(res);

        }

        [HttpGet("/bots/{listOfIds}")]
        public async Task<List<BotEventType>> GetSubscriptionsByIdAsync(string listOfIds)
        {
            string[] bots = listOfIds.Split(',');
            List<BotEventType> res = await _context.Subscriptions.Where(s => bots.Contains(s.BotId)).ToListAsync();
            return res;
        }

        [HttpPost("/bots")]
        public async Task<IActionResult> AddMoreBotsAsync()
        {
            for (int i = 0; i < 1000; i++)
            {
                var id = Guid.NewGuid();
                await _context.Subscriptions.AddAsync(new BotEventType 
                {
                    Id = id, 
                    BotId = id.ToString(), 
                    EventType = "Microsoft.Communication.CallParticipantAdded, Microsoft.Communication.CallParticipantRemoved" 
                });                
            }
            _context.SaveChanges();
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
