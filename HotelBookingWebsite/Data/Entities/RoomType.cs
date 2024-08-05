using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using HotelBookingWebsite.Data;
using HotelBookingWebsite.Data.Entities;

namespace HotelBookingWebsite.Data.Entities
{
    public class RoomType
    {
        public short Id { get; set; }
        [Required,MaxLength(100),Unicode(false)]
        public string? Name { get; set; }
        [Required, MaxLength(100),Unicode(false)]
        public string? Image { get; set; }
        [Required, Range(1, double.MaxValue)]
        public decimal Price { get; set; }
        [Required, MaxLength(200)]
        public string? Description { get; set; }
        public int MaxAdult { get; set; }
        public int MaxChildren {  get; set; }
        public bool IsActive { get; set; }
        public DateTime? AddedOn { get; set; }
        [Required]
        public string? AddedBy { get; set; }
        public DateTime? LastUpdatedOn { get; set; }
        public string? LastUpdatedBy { get; set; }

        [ForeignKey(nameof(AddedBy))]
        public virtual ApplicationUser AddedByUser { get; set; }
        public virtual ICollection<RoomTypeSideOption> SideOptions {  get; set; } = new List<RoomTypeSideOption>(); 
        public virtual ICollection<Room> Rooms { get; set; }
        

    }
}
