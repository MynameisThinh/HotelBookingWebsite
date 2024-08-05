using HotelBookingWebsite.Constants;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingWebsite.Models
{
    public class BookingDisplayModel
    {
        public long Id { get; set; }

        public short RoomTypeId { get; set; }
        public string RoomTypeName { get; set; }
        public int? RoomId { get; set; }
        public string? RoomNumber { get; set; }
        public string GuestId { get; set; }
        public string GuestName { get; set; }
        public int Adult { get; set; }
        public int Children { get; set; }
        public DateOnly CheckInDate { get; set; }
        public DateOnly CheckOutDate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime BookedOn { get; set; }
        public string? SpecialRequest { get; set; }
        public string? Remarks { get; set; }

        public BookingStatus Status { get; set; }

        public bool IsRoomNumberAssign => Status == BookingStatus.PaymentSuccess || Status == BookingStatus.Booked;
        public bool CanApproved => Status == BookingStatus.PaymentSuccess;
        public bool CanCancel=> Status != BookingStatus.PaymentCancelled && Status != BookingStatus.Cancelled;

        public bool CanMakePayment => (Status == BookingStatus.Pending || Status == BookingStatus.PaymentCancelled)
                                     && CheckInDate >= DateOnly.FromDateTime(DateTime.Today);
    }
}
