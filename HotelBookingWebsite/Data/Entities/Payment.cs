using System.ComponentModel.DataAnnotations;

namespace HotelBookingWebsite.Data.Entities
{
    public class Payment
    {
        public Guid Id { get; set; }
        
        public long BookingId { get; set; }
        [MaxLength(150)]
        public string CheckoutSessionId { get; set; }
        [MaxLength(150)]
        public string? AdditionalInfo { get; set; }
        [MaxLength(50)]
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual Booking Booking { get; set; }
    }
}
