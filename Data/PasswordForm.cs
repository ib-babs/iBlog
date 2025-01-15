using System.ComponentModel.DataAnnotations;

namespace iBlog.Data
{
    public class PasswordForm
    {
        [DataType(DataType.Password)]
        [Required]
        //[StringLength(16)]
        public string OldPassword { get; set; } = String.Empty;

        [DataType(DataType.Password)]
        //[StringLength(16)]
        [Required]
        public string NewPassword { get; set; } = String.Empty;
    }
}
