using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DatingOpg.Models
{
    public class Like
    {
        [Key]
        public int LikeId { get; set; }

        [ForeignKey("Account")]
        public int SenderId { get; set; }
        [ForeignKey("Profile")]
        public int ReceiverId { get; set; }
        public Account Sender { get; set; }
        public Profile Receiver { get; set; }
        public int status { get; set; } = 0;
    }
}
