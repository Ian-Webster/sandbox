using Messaging.Outbox.Domain.Messages;

namespace Messaging.Outbox.Consumer2.Services;

public interface IMessageService
{
    bool AddMessage(OutboxMessageBase message);

    IEnumerable<OutboxMessageBase> GetMessages();
}