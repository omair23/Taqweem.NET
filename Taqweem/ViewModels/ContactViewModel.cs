using System.ComponentModel.DataAnnotations;

namespace Taqweem.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Location { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string SecurityQuestion { get; set; }
    }
}