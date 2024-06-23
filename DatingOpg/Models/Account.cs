using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;

namespace DatingOpg.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Email { get; set; }

        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        [DefaultValue("User")]
        [MaxLength(20)]
        [HiddenInput(DisplayValue = false)]
        [Column(TypeName = "nvarchar(20)")]
        public string Role { get; set; } = "User";

        public Profile Profile { get; set; }
        public ICollection<Like> SentLikes { get; set; }
        public ICollection<Chat> SentChats { get; set; }
    }
}
