using GigaChatBot.Domain.Enums;
using MediatR;

namespace GigaChatBot.Application.Message.Commands.ReactToMessage
{
    public class ReactToMessageCommand : IRequest
    {
        public Guid MessageId { get; set; }
        public MessageReaction Reaction { get; set; }
    }
}
