using System.ComponentModel.DataAnnotations;

namespace ClassLibrary1.DTOs
{
    /// <summary>
    /// Username, Password, Email, DisplayName
    /// </summary>
    public class RegisterDto
    {
        public string? DisplayName { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        //[RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")]
        public string? Password { get; set; }

        [Required]
        public string? UserName { get; set; }
    }
}