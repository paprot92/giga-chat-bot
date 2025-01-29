namespace GigaChatBot.Application.Common.Interfaces.Services
{
    public interface IChatBotClient
    {
        IAsyncEnumerable<string> GetMessageResponseStreamAsync(string userMessage, CancellationToken cancellationToken);
    }
}
