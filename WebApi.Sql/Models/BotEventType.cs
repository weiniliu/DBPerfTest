using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.Sql.Models
{
    public class BotEventType
    {
        [Key]
        public Guid Id { get; set; }
        public string EventType { get; set; }
        public string BotId { get; set; }
    }
}
