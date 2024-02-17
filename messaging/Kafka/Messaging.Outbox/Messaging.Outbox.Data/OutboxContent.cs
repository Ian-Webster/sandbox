using Messaging.Outbox.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Outbox.Data
{
    public class OutboxContent :DbContext
    {
        public OutboxContent(DbContextOptions options) : base(options) { }

        override protected void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseNpgsql("Host=localhost:5432;Username=postgres;Password=postgres;Database=outbox")
                .UseSnakeCaseNamingConvention();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresEnum<MessageStatus>("message_status_enum");
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OutboxContent).Assembly);
        }

    }
}
