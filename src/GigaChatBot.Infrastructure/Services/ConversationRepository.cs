using GigaChatBot.Application.Common.Interfaces.Services;
using GigaChatBot.Domain.Entities;
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

        public async Task<Conversation?> GetConversationAsync(Guid conversationId)
        {
            return await _context.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == conversationId);
        }


        public async Task UpdateConversation(Conversation conversation)
        {
            _context.Conversations.Update(conversation);
            await _context.SaveChangesAsync();
        }

        public async Task SaveMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
