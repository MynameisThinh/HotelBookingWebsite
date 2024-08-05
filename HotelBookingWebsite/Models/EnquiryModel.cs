using System.ComponentModel.DataAnnotations;

namespace HotelBookingWebsite.Models
{
    public class EnquiryModel
    {
        [Required,MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(50),EmailAddress]   
        public string Email { get; set; }
        [Required, MaxLength(50)]
        public string Subject { get; set; }
        [Required, MaxLength(250)]
        public string Message { get; set; }
    }
}
