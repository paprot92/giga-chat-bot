using GigaChatBot.Application.Common.Interfaces.Services;
using GigaChatBot.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GigaChatBot.Infrastructure
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
			services.AddSingleton<IChatBotService, LoremChatBotService>();
			services.AddScoped<IConversationRepository, ConversationRepository>();
            return services;
		}
	}
}