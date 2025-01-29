namespace GigaChatBot.Domain.Entities
{
    public class Conversation
    {
        public Guid Id { get; set; }
        public List<Message> Messages { get; set; } = [];
    }
}
