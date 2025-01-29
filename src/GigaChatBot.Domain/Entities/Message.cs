using GigaChatBot.Domain.Enums;
using System.Text.Json.Serialization;

namespace GigaChatBot.Domain.Entities
{
    public class Message
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public MessageType Type { get; set; }
        public MessageReaction Reaction { get; set; } = MessageReaction.None;
        public Guid ConversationId { get; set; }

        [JsonIgnore]
        public Conversation? Conversation { get; set; }
    }
}
