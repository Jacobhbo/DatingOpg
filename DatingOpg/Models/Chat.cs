using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DatingOpg.Models
{
    public class Chat
    {
        [Key]
        public int ChatId { get; set; }

        [ForeignKey("Account")]
        public int SenderId { get; set; }
        public Account Sender { get; set; }

        [ForeignKey("Account")]
        public int ReceiverId { get; set; }
        public Account Receiver { get; set; }

        [Required]
        public string Message { get; set; }

        public int Status { get; set; }
    }
}
