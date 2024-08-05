using System.ComponentModel.DataAnnotations;

namespace HotelBookingWebsite.Models
{
    public class ChangePassModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }


        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; } = "";


        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare(nameof(NewPassword), ErrorMessage = "Mật khẩu và xác nhận mật khẩu không khớp nhau")]
        public string ConfirmNewPassword { get; set; } = "";
    }
}
