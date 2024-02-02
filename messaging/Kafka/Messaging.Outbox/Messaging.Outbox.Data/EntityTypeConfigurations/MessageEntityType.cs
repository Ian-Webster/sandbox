using Messaging.Outbox.Data.Entities;
using Messaging.Outbox.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Messaging.Outbox.Data.EntityTypeConfigurations
{
    public class MessageEntityType : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(x => x.MessageId);

            builder.ToTable("message", "public");
        }
    }
}
