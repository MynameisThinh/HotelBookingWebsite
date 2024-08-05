using System.ComponentModel.DataAnnotations;

namespace HotelBookingWebsite.Data.Entities
{
    public class Subscriber
    {
        [Key]
        public long Id { get; set; }
        [Required,MaxLength(50)]
        public string Email { get; set; }
        public DateTime SubscribeOn { get; set; }
    }
}
