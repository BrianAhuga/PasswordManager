using System.ComponentModel.DataAnnotations;

namespace PasswordManager.Models
{
    public class PasswordEntry
    {
        public int Id { get; set; }

        [Required]
        public string Site { get; set; }

        [Required]
        [Display(Name = "Owner")]
        public string PasswordFor { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required, EmailAddress]
        public string EmailAddress { get; set; }

        public string? Password { get; set; }
    }
}
