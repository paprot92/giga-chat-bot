using GigaChatBot.Application.Common.Interfaces.Services;
using MediatR;

namespace GigaChatBot.Application.Message.Commands.ReactToMessage
{
    public class ReactToMessageHandler : IRequestHandler<ReactToMessageCommand>
    {
        private readonly IConversationRepository _conversationRepository;

        public ReactToMessageHandler(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task Handle(ReactToMessageCommand request, CancellationToken cancellationToken)
        {
            await _conversationRepository.ReactToMessageAsync(request.Reaction, request.MessageId);
        }
    }
}
