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
        Long,
        VeryLong
    }

    public class LoremChatBotService : IChatBotService
    {
        private static readonly Random _random = new();
        private readonly IConversationRepository _conversationRepository;
        private readonly IConversationNotificationService _notificationService;

        public LoremChatBotService(IConversationRepository conversationRepository, IConversationNotificationService notificationService)
        {
            _conversationRepository = conversationRepository;
            _notificationService = notificationService;
        }

        public async Task SendMessageAsync(Guid conversationId, string message, CancellationToken cancellationToken)
        {
            var conversation = await _conversationRepository.GetConversationAsync(conversationId);
            if (conversation is null)
            {
                throw new Exception("Conversation not found.");
            }

            await _conversationRepository.AddMessageAsync(
                new Message { ConversationId = conversationId, Content = message, CreatedOn = DateTime.Now, Type = Domain.Enums.MessageType.User });

            var responseStream = GetMessageResponseStreamAsync(message, cancellationToken);
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
            catch (Exception) {
                responseBuilder.Clear();
                responseBuilder.Append("An error occurred during generation. Try again.");
            }
            finally
            {
                await _conversationRepository.AddMessageAsync(
                    new Message { ConversationId = conversationId, Content = responseBuilder.ToString(), CreatedOn = DateTime.Now, Type = Domain.Enums.MessageType.Assistant });
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
            },
            {
                ResponseType.VeryLong,
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec ut ex felis. Integer pharetra, augue ut cursus tincidunt, justo lacus facilisis risus, et maximus urna metus et ligula. Nullam aliquam, augue id feugiat consequat, sapien libero aliquet lorem, vel accumsan libero sem vel felis. Nunc euismod et elit eu scelerisque. In vulputate interdum lorem, id facilisis erat vulputate eu. Curabitur mollis, metus nec gravida finibus, ligula sapien cursus orci, nec hendrerit risus felis id justo. Fusce volutpat mollis auctor. Phasellus molestie urna at mi volutpat, eu faucibus libero consequat.\n\n" +
                "Sed ultricies arcu eu metus malesuada, eu laoreet enim vehicula. Nam interdum interdum diam, et convallis nunc pretium eget. In tincidunt, dui ac hendrerit sollicitudin, risus lectus vulputate nisi, a euismod velit sapien vel justo. Etiam molestie, arcu sit amet ullamcorper tristique, metus odio volutpat nulla, a elementum enim ipsum nec ante. Sed placerat convallis risus sed volutpat. Nulla facilisi. Pellentesque a scelerisque nunc, id feugiat erat. Integer sit amet libero at arcu pretium malesuada. Donec lobortis mi vitae lacus hendrerit laoreet.\n\n" +
                "Aenean tempor metus sit amet erat interdum, et laoreet lectus ullamcorper. Quisque fringilla sollicitudin nisl et feugiat. Fusce id elit ut ante tincidunt gravida ac eu orci. Vivamus auctor nisl eu nisl malesuada tempus. Curabitur a arcu tincidunt, feugiat felis ut, interdum elit. Donec varius ac felis id volutpat. Vestibulum at nisl sit amet enim fringilla sodales. Etiam condimentum auctor nisl non congue. Integer interdum, arcu id luctus lobortis, ipsum tortor hendrerit velit, at interdum ligula enim ac leo.\n\n" +
                "Mauris suscipit ligula ut sem dictum, ac vestibulum turpis elementum. Nam varius suscipit eros. Sed venenatis, lorem vel ultricies convallis, purus arcu luctus odio, eget luctus metus risus vel nisi. Ut malesuada cursus enim, vel tincidunt tortor. Fusce tristique magna et neque vestibulum, sed tincidunt ligula mollis. In hac habitasse platea dictumst. Mauris ac massa urna. Nam nec neque tincidunt, tincidunt felis ut, tristique sem. Fusce malesuada nisi non est sollicitudin, et feugiat turpis pellentesque. Cras eget auctor velit. Nulla vestibulum turpis et turpis dapibus fermentum.\n\n" +
                "Fusce bibendum risus at tortor fermentum, ac ullamcorper velit sollicitudin. Suspendisse potenti. Curabitur ac nisl nec tortor malesuada mollis sit amet sit amet dui. Phasellus sed leo ipsum. Aliquam vitae turpis sed arcu tincidunt accumsan. Mauris volutpat, odio ac aliquet tincidunt, ligula neque faucibus dui, vel laoreet eros elit sed risus. Curabitur id nisi urna. Sed ultricies libero eget risus interdum malesuada. Aenean dictum sit amet lorem et eleifend. Nunc vehicula ex et elit laoreet, at iaculis felis pretium. Nam vel orci magna.\n\n" +
                "Vestibulum non metus quam. Nulla ac justo sem. Vivamus sed risus sit amet libero tristique euismod vel sed ante. Integer dictum interdum augue ut fringilla. Nam elementum auctor orci at fermentum. Morbi ut nulla ac elit rutrum tempor. Ut pellentesque velit nisi, id vulputate sapien posuere ut. Cras vehicula libero risus, in ultricies ante aliquam vitae. Pellentesque et neque sit amet lorem tempor dapibus. In ac ante sit amet ipsum vehicula pharetra. Sed at erat vel risus lacinia tristique. Etiam sit amet bibendum odio. Etiam fringilla ex a tortor aliquet, eget dictum nunc fermentum. Integer pellentesque lacus nec nunc feugiat, vel lobortis nunc facilisis."
            }
        };
    }
}
