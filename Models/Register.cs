using System.ComponentModel.DataAnnotations;

namespace MovieOnlineBookingMVC.Models
{
    public class Register
    {
        [Key]
        public int UserId { get; set; }
        [Required]
        public string? Name { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string? Email { get; set; }


        [Required(ErrorMessage = "Password is required.")]
        [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
        //[RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&])[A-Za-z\d@$!%*#?&]{8,}$",
        //  ErrorMessage = "Password must contain at least 8 characters, one letter, one number, and one special character.")]

        public string? Password { get; set; }
        public string Role { get; internal set; }
    }
}
