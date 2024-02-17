using Messaging.Outbox.Domain.Messages;

namespace Messaging.Outbox.Business.Services;

public class TopicNameResolver: ITopicNameResolver
{
    public string GetTopicForMessageType(Type messageType)
    {
        switch (messageType.Name)
        {
            case nameof(HelloAll):
                return "Outbox-HelloAll";
            case nameof(HelloConsumer1):
                return "Outbox-HelloConsumer1";
            case nameof(HelloConsumer2):
                return "Outbox-HelloConsumer2";
        }
        throw new ArgumentException("Message type not found");
    }
}