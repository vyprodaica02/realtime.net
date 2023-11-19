using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Realtime.Entity
{
    public class Message : BaseEnity
    {
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
        public int? SenderUserId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public User? SenderUser { get; set; }
        public int? ReceiverUserId { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public User? ReceiverUser { get; set; }
    }
}
