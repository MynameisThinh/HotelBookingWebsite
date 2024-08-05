using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelBookingWebsite.Data.Entities
{
    public class Enquiry
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required, MaxLength(50), EmailAddress]
        public string Email { get; set; }
        [Required, MaxLength(50)]
        public string Subject { get; set; }
        [Required, MaxLength(250)]
        public string Message { get; set; }

        public DateTime EnquireOn { get; set; }
    }
}
