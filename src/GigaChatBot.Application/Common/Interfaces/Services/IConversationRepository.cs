using GigaChatBot.Domain.Entities;

namespace GigaChatBot.Application.Common.Interfaces.Services
{
    public interface IConversationRepository
    {
        Task<Domain.Entities.Conversation> GetTestConversationAsync();
        Task<Domain.Entities.Conversation> CreateConversationAsync();
        Task<Domain.Entities.Conversation?> GetConversationAsync(Guid conversationId);
        Task UpdateConversation(Domain.Entities.Conversation conversation);
        Task SaveMessageAsync(Message message);
    }
}
