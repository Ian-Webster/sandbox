namespace Messaging.Outbox.Business.Services;

/// <summary>
/// Used to resolve the topic name for a given message type.
/// </summary>
public interface ITopicNameResolver
{
    string GetTopicForMessageType(Type messageType);
}