using Messaging.Outbox.Data.Entities;

namespace Messaging.Outbox.Data.Repositories
{
    public interface IMessageRepository
    {
        Task<bool> AddMessage(Message message, CancellationToken token);

        Task<bool> UpdateMessage(Message message, CancellationToken token); 

        Task<IEnumerable<Message>> GetUnSentMessages(CancellationToken token);
    }
}
