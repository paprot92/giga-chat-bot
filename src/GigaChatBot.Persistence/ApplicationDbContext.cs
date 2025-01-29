using GigaChatBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GigaChatBot.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
    }
}