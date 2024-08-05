using System.ComponentModel.DataAnnotations;
using HotelBookingWebsite.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingWebsite.Data.Entities
{
    public class Room
    {
        [Key]

        public int ID { get; set; }
        public short RoomTypeId { get; set; }
        [Required, MaxLength(25),Unicode(false)]
        public string? RoomNumber { get; set; }
        public bool IsAvailable { get; set; }

        public bool IsDeleted { get; set; }
        public virtual RoomType RoomType { get; set; }

    }
}
