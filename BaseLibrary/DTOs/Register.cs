using System.ComponentModel.DataAnnotations;

namespace BaseLibrary.DTOs
{
    public class Register : AccountBase
    {
        [Required]
        [MinLength(5, ErrorMessage = "Full name must be at least 5 characters long.")]
        [MaxLength(50, ErrorMessage = "Full name cannot exceed 50 characters.")]
        public string? FullName { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        [Required(ErrorMessage = "Password is required.")]
        public string? ConfirmPassword { get; set; }
    }
}
