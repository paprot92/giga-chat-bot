using GigaChatBot.Application.Common.Interfaces.Services;
using MediatR;

namespace GigaChatBot.Application.Conversation.Queries.GetTestConversationDetails
{
    public class GetTestConversationDetailsHandler : IRequestHandler<GetTestConversationDetailsQuery, Domain.Entities.Conversation>
    {
        private readonly IConversationRepository _conversationRepository;

        public GetTestConversationDetailsHandler(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task<Domain.Entities.Conversation> Handle(GetTestConversationDetailsQuery request, CancellationToken cancellationToken)
        {
            return await _conversationRepository.GetTestConversationAsync();
        }
    }
}
