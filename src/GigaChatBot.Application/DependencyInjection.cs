using GigaChatBot.Application.Common.Interfaces.Services;
using GigaChatBot.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace GigaChatBot.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddMediatR((configuration) => configuration.RegisterServicesFromAssembly(typeof(AssemblyReference).Assembly));
			services.AddScoped<IChatBotService, ChatBotService>();
            return services;
		}
	}
}