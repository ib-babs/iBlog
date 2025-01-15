using System.ComponentModel.DataAnnotations;

namespace iBlog.Data
{
    public class EditProfileForm
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = String.Empty;
        [Required]
        public string UserName { get; set; } = String.Empty;
    }
}
