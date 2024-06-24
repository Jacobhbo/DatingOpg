using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace DatingOpg.Models
{
    public class Profile
    {
        [Key]
        public int ProfileId { get; set; }

        public int Height { get; set; }
        public int Weight { get; set; }
        public DateTime BirthDate { get; set; }

        [MaxLength(50)]
        public string NickName { get; set; }

        [MaxLength(10)]
        public string Gender { get; set; }

        [ForeignKey("Account")]
        public int AccountId { get; set; }
        public Account Account { get; set; }

        public ICollection<Like> ReceivedLikes { get; set; }

        // Add this property to include received chats
        public ICollection<Chat> ReceivedChats { get; set; }
    }
}
