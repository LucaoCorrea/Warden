using System.ComponentModel.DataAnnotations.Schema;

namespace Warden.Models
{
    public class ChatModel
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime SentAt { get; set; }

        public string UserName { get; set; }

    }
}
