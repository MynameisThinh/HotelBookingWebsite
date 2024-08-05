using System.ComponentModel.DataAnnotations;

namespace HotelBookingWebsite.Models
{
    public class RegisterUserModel 
    {
        [Required, MaxLength(15)]
        public string? FirstName { get; set; } = "";
        [MaxLength(15)]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required, MaxLength(15), RegularExpression(@"^[0-9\+\(\)\s]+$")]
        public string? ContactNumber { get; set; } = "";
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";
        [MaxLength (50)]
        public string? Designation { get; set; }
    }
}
