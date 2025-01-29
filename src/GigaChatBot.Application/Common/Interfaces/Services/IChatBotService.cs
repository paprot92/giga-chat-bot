
namespace GigaChatBot.Application.Common.Interfaces.Services
{
    public interface IChatBotService
    {
        Task SendMessageAsync(Guid conversationId, string userMessage, CancellationToken cancellationToken);
    }
}
