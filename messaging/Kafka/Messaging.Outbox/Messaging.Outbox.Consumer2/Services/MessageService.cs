using System.Collections.Concurrent;
using Messaging.Outbox.Domain.Messages;

namespace Messaging.Outbox.Consumer2.Services
{
    public class MessageService: IMessageService
    {
        private ConcurrentDictionary<Guid, OutboxMessageBase> _messages;

        public MessageService()
        {
            _messages = new ConcurrentDictionary<Guid, OutboxMessageBase>();
        }

        public bool AddMessage(OutboxMessageBase message)
        {
            if (_messages.TryAdd(Guid.NewGuid(), message))
            {
                return true;
            }
            return false;
        }

        public IEnumerable<OutboxMessageBase> GetMessages()
        {
            return _messages.Values;
        }
    }
}
