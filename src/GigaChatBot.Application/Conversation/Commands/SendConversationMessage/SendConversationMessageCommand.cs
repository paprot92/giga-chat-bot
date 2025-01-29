using MediatR;

namespace GigaChatBot.Application.Conversation.Commands.SendConversationMessage
{
    public class SendConversationMessageCommand : IRequest
    {
        public Guid ConversationId { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}
