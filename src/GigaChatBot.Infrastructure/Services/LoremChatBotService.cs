using GigaChatBot.Application.Common.Interfaces.Services;
using GigaChatBot.Domain.Entities;
using System.Runtime.CompilerServices;
using System.Text;

namespace GigaChatBot.Infrastructure.Services
{
    public enum ResponseType
    {
        Short,
        Medium,
        Long
    }

    public class LoremChatBotService : IChatBotService
    {
        private static readonly Random _random = new();
        private readonly IConversationRepository _conversationRepository;

        public LoremChatBotService(IConversationRepository conversationRepository)
        {
            _conversationRepository = conversationRepository;
        }

        public async Task SendMessageAsync(Guid conversationId, string message, CancellationToken cancellationToken)
        {
            var conversation = await _conversationRepository.GetConversationAsync(conversationId);
            if (conversation is null)
            {
                throw new Exception("Conversation not found.");
            }

            conversation.Messages.Add(new Message { Content = message, CreatedOn = DateTime.Now, Type = Domain.Enums.MessageType.User });

            var responseStream = GetMessageResponseStreamAsync(message, cancellationToken);
            var responseBuilder = new StringBuilder();
            try
            {
                await foreach (var chunk in responseStream.WithCancellation(cancellationToken))
                {
                    // todo: send chunk to signalR hub
                    responseBuilder.Append(' ').Append(chunk);
                }
            }
            catch (OperationCanceledException) { }
            catch (Exception) {
                responseBuilder.Clear();
                responseBuilder.Append("An error occurred during generation. Try again.");
            }
            finally
            {
                conversation.Messages.Add(new Message { Content = responseBuilder.ToString(), CreatedOn = DateTime.Now, Type = Domain.Enums.MessageType.Assistant });
                await _conversationRepository.UpdateConversation(conversation);
            }
        }

        public async IAsyncEnumerable<string> GetMessageResponseStreamAsync(string userMessage, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            var responseType = DrawResponseType();
            var response = GigaChatBotResources.ResponseTypeToResponses[responseType];

            foreach (var word in response.Split(' '))
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    yield break; 
                }

                yield return word;
                await Task.Delay(100, cancellationToken);
            }
        }

        private static ResponseType DrawResponseType()
        {
            var values = Enum.GetValues<ResponseType>().ToArray();
            return values[_random.Next(values.Length)];
        }
    }

    public static class GigaChatBotResources
    {
        public static readonly Dictionary<ResponseType, string> ResponseTypeToResponses = new()
        {
            {
                ResponseType.Short,
                "Lorem ipsum dolor sit amet."
            },
            {
                ResponseType.Medium,
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat."
            },
            {
                ResponseType.Long,
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus cursus euismod dui, et sollicitudin risus gravida non. Sed tristique dui at purus facilisis, a suscipit ante condimentum. Nulla euismod consequat nisi, sit amet euismod arcu posuere non.\n\n" +
                "Phasellus convallis urna eu urna vehicula, id cursus lorem auctor. Ut tincidunt convallis velit non egestas. Integer sollicitudin orci nec lectus convallis, vel vestibulum velit cursus. Sed volutpat, felis eu auctor bibendum, ante risus vehicula nulla, vel volutpat mauris augue eu nunc.\n\n" +
                "Nam sit amet justo vestibulum, efficitur sapien eget, condimentum enim. Aenean consequat gravida erat, at convallis libero auctor vel. Sed convallis, erat ac placerat maximus, neque orci aliquet ipsum, vel gravida enim felis at neque. Integer vulputate fringilla nisi, a varius lectus porttitor non."
            }
        };
    }
}
