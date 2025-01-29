using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GigaChatBot.Persistence
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration config)
		{
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(config.GetConnectionString("DefaultConnection")));

            return services;
		}
	}
}