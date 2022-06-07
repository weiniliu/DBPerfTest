using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace WebApi.Sql.Models
{
    public class BotSubscriptionContext : DbContext
    {
        public BotSubscriptionContext(DbContextOptions<BotSubscriptionContext> options) : base(options)
        {
        }

        public DbSet<BotEventType> Subscriptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BotEventType>().ToTable("BotEventType", "dbo")
                .HasKey("Id");
        }
    }
}
