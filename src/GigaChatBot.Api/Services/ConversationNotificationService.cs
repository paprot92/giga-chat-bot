using GigaChatBot.Api.Hubs;
using GigaChatBot.Application.Common.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;

namespace GigaChatBot.Api.Services
{
    public class ConversationNotificationService : IConversationNotificationService
    {
        private readonly IHubContext<ConversationHub> _hubContext;

        public ConversationNotificationService(IHubContext<ConversationHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public async Task SendConversationMessageChunk(Guid conversationId, string chunk)
        {
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", conversationId, chunk);
        }
    }
}
