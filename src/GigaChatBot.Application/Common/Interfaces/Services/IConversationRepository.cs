using GigaChatBot.Domain.Entities;

namespace GigaChatBot.Application.Common.Interfaces.Services
{
    public interface IConversationRepository
    {
        Task<Conversation> CreateConversationAsync();
        Task<Conversation?> GetConversationAsync(Guid conversationId);
        Task UpdateConversation(Conversation conversation);
        Task SaveMessageAsync(Message message);
    }
}
