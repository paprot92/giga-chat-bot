using GigaChatBot.Application.Common.Interfaces.Services;
using GigaChatBot.Domain.Entities;
using GigaChatBot.Domain.Enums;
using GigaChatBot.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GigaChatBot.Infrastructure.Services
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ApplicationDbContext _context;

        public ConversationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Conversation> CreateConversationAsync()
        {
            var conversation = new Conversation();
            await _context.Conversations.AddAsync(conversation);
            await _context.SaveChangesAsync();
            return conversation;
        }

        public async Task<Conversation> GetTestConversationAsync()
        {
            var conversation = await _context.Conversations
                .AsNoTracking()
                .Include(c => c.Messages)
                .FirstOrDefaultAsync();
            if (conversation is null)
            {
                return await CreateConversationAsync();
            }
            return conversation;
        }

        public async Task<Conversation?> GetConversationAsync(Guid conversationId)
        {
            return await _context.Conversations
                .AsNoTracking()
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == conversationId);
        }

        public async Task ReactToMessageAsync(MessageReaction reaction, Guid messageId)
        {
            var message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
            if (message is null)
            {
                throw new Exception("Message not found.");
            }
            message.Reaction = reaction;
            await _context.SaveChangesAsync();
        }

        public async Task AddMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
