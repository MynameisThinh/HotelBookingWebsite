using System.ComponentModel.DataAnnotations;
using HotelBookingWebsite.Constants;
using HotelBookingWebsite.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingWebsite.Data.Entities
{
    public class Booking
    {
        [Key]
        public long Id { get; set; }

        public short RoomTypeId { get; set; }   
        public int? RoomId { get; set; }
        [Required]
        public string GuestId { get; set; }
        public int Adult { get; set; }
        public int Children {  get; set; } 
        public DateOnly CheckInDate {  get; set; }
        public DateOnly CheckOutDate { get; set; }
        [Range(1,double.MaxValue)]
        public decimal TotalAmount { get; set; }
        public DateTime BookedOn {  get; set; }

        [MaxLength(150)]
        public string? SpecialRequest { get; set; }
        [MaxLength(150)]
        public string? Remarks { get; set; }

        public BookingStatus Status { get; set; } = BookingStatus.Pending;
        public virtual RoomType RoomType { get; set; }
        public virtual Room Room { get; set; }
        public virtual ApplicationUser Guest { get; set; }

    }
}
