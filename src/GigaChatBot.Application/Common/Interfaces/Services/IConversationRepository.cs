using GigaChatBot.Domain.Enums;

namespace GigaChatBot.Application.Common.Interfaces.Services
{
    public interface IConversationRepository
    {
        Task<Domain.Entities.Conversation> GetTestConversationAsync();
        Task<Domain.Entities.Conversation> CreateConversationAsync();
        Task<Domain.Entities.Conversation?> GetConversationAsync(Guid conversationId);
        Task AddMessageAsync(Domain.Entities.Message message);
        Task ReactToMessageAsync(MessageReaction reaction, int messageId);
    }
}
