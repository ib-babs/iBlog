using System.ComponentModel.DataAnnotations;

namespace iBlog.Data
{
    public class LoginUserIdentityData
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = String.Empty;
        [DataType(DataType.Password)]
        [Required]
        [StringLength(16)]
        public string Password { get; set; } = String.Empty;
    }
}
