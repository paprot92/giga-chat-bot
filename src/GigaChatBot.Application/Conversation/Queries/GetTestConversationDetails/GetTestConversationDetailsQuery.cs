using MediatR;

namespace GigaChatBot.Application.Conversation.Queries.GetTestConversationDetails
{
    public class GetTestConversationDetailsQuery : IRequest<Domain.Entities.Conversation> { }
}
