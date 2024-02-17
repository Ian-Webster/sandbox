using Messaging.Outbox.Data.Entities;
using Messaging.Outbox.Data.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Messaging.Outbox.Data.EntityTypeConfigurations
{
    public class MessageEntityType : IEntityTypeConfiguration<Message>
    {
        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder.HasKey(p => p.MessageId);


            builder.Property(e => e.Status)
                   .HasColumnName("status_id");

            builder.ToTable("message", "public");
        }
    }
}
