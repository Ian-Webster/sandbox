﻿using DataAccess.Repository;
using Messaging.Outbox.Data.Entities;
using Messaging.Outbox.Data.Enums;
using Microsoft.EntityFrameworkCore;

namespace Messaging.Outbox.Data.Repositories
{
    public class MessageRepository : IMessageRepository
    {
        private readonly IRepository<Message> _messageRepo;

        public MessageRepository(RepositoryFactory<OutboxContent> repositoryFactory)
        {
            _messageRepo = repositoryFactory.GetRepositoryByType<Message>();
        }

        public async Task<IEnumerable<Message>> GetUnSentMessages(CancellationToken token)
        {
            var unsentStatuses = new List<MessageStatus> { MessageStatus.NotPersisted, MessageStatus.PossiblyPersisted };
            return await _messageRepo.DbSet
                .Where(x => unsentStatuses.Contains(x.Status)).ToListAsync(token);
        }

        public async Task<bool> AddMessage(Message message, CancellationToken token)
        {
            return await _messageRepo.Add(message, token);
        }

        public Task<bool> UpdateMessage(Message message, CancellationToken token)
        {
            var messageToUpdate = _messageRepo.DbSet.FirstOrDefault(x => x.MessageId == message.MessageId);
            if (messageToUpdate == null)
            {
                return Task.FromResult(false);
            }
            
            messageToUpdate.Status = message.Status;
            messageToUpdate.SentDate = message.SentDate;
            messageToUpdate.Error = message.Error;
            return _messageRepo.Update(messageToUpdate, token);
        }
    }
}