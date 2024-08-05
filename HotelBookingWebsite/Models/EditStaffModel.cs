using System.ComponentModel.DataAnnotations;

namespace HotelBookingWebsite.Models
{
    public class MyProfileModel
    {

        public string Id { get; set; }
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
       
        [MaxLength(50)]
        public string? Designation { get; set; }
    }
}
