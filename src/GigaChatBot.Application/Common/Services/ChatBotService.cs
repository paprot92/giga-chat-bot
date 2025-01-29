using GigaChatBot.Application.Common.Interfaces.Services;
using System.Text;

namespace GigaChatBot.Application.Services
{
    public class ChatBotService : IChatBotService
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IConversationNotificationService _notificationService;
        private readonly IChatBotClient _chatBotClient;

        public ChatBotService(IConversationRepository conversationRepository, IConversationNotificationService notificationService, IChatBotClient chatBotClient)
        {
            _conversationRepository = conversationRepository;
            _notificationService = notificationService;
            _chatBotClient = chatBotClient;
        }

        public async Task SendMessageAsync(Guid conversationId, string message, CancellationToken cancellationToken)
        {
            var conversation = await _conversationRepository.GetConversationAsync(conversationId);
            if (conversation is null)
            {
                throw new Exception("Conversation not found.");
            }

            await _conversationRepository.AddMessageAsync(
                new Domain.Entities.Message { ConversationId = conversationId, Content = message, CreatedOn = DateTime.Now, Type = Domain.Enums.MessageType.User });

            var responseStream = _chatBotClient.GetMessageResponseStreamAsync(message, cancellationToken);
            var responseBuilder = new StringBuilder();
            try
            {
                await foreach (var chunk in responseStream.WithCancellation(cancellationToken))
                {
                    await _notificationService.SendConversationMessageChunk(conversationId, $" {chunk}");
                    responseBuilder.Append($" {chunk}");
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception)
            {
                responseBuilder.Clear();
                responseBuilder.Append("An error occurred during generation. Try again.");
            }
            finally
            {
                await _conversationRepository.AddMessageAsync(
                    new Domain.Entities.Message { ConversationId = conversationId, Content = responseBuilder.ToString(), CreatedOn = DateTime.Now, Type = Domain.Enums.MessageType.Assistant });
            }
        }
    }
}
