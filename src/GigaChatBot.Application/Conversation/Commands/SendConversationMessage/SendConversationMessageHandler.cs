using GigaChatBot.Application.Common.Interfaces.Services;
using MediatR;

namespace GigaChatBot.Application.Conversation.Commands.SendConversationMessage
{
    public class SendConversationMessageHandler : IRequestHandler<SendConversationMessageCommand>
    {
        private readonly IChatBotService _chatBotService;

        public SendConversationMessageHandler(IChatBotService chatBotService)
        {
            this._chatBotService = chatBotService;
        }

        public async Task Handle(SendConversationMessageCommand request, CancellationToken cancellationToken)
        {
            await _chatBotService.SendMessageAsync(request.ConversationId, request.Content, cancellationToken);
        }
    }
}
