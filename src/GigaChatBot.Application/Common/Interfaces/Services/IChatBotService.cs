using GigaChatBot.Domain.Entities;

namespace GigaChatBot.Application.Common.Interfaces.Services
{
    public interface IChatBotService
    {
        Task SendMessageAsync(Guid conversationId, string userMessage, CancellationToken cancellationToken);
    }
}
