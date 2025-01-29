using GigaChatBot.Domain.Enums;
using MediatR;

namespace GigaChatBot.Application.Message.Commands.ReactToMessage
{
    public class ReactToMessageCommand : IRequest
    {
        public int MessageId { get; set; }
        public MessageReaction Reaction { get; set; }
    }
}
