namespace GigaChatBot.Application.Common.Interfaces.Services
{
    public interface IConversationNotificationService
    {
        Task SendConversationMessageChunk(Guid conversationId, string message);
    }
}
