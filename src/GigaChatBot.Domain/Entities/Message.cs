using GigaChatBot.Domain.Enums;

namespace GigaChatBot.Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public MessageType Type { get; set; }
        public MessageUserReaction Reaction { get; set; } = MessageUserReaction.None;
    }
}
