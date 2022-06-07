using Newtonsoft.Json;
using System.Collections.Generic;

namespace WebApi.Cosmos.Models
{
    public class BotEventType
    {
        [JsonProperty(PropertyName = "id")]
        public string BotId { get; set; }
        [JsonProperty(PropertyName = "eventTypes")]
        public List<string> EventTypes { get; set; }
    }
}
