using GigaChatBot.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GigaChatBot.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Conversation> Conversations { get; set; }
        public DbSet<Message> Messages { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);
        }
        
    }
}