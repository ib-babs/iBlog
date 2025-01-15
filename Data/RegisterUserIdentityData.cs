using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace iBlog.Data
{
    public class RegisterUserIdentityData
    {
        [EmailAddress]
        [Required]
        public string Email { get; set; } = String.Empty;
        [Required]
        public string UserName { get; set; } = String.Empty;

        [DataType(DataType.Password)]
        [Required]
        [StringLength(16)]
        public string Password { get; set; } = String.Empty;
    }
}
