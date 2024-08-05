using HotelBookingWebsite.Data.Entities;
using HotelBookingWebsite.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingWebsite.Models
{
    public class RoomTypeSaveModel
    {
        public short Id { get; set; }

        [Required, MaxLength(100), Unicode(false)]
        public string? Name { get; set; }
        [MaxLength(100)]
        public string? Image { get; set; }

        
        [Required, Range(1, double.MaxValue)]
        public decimal Price { get; set; }
        [Required, MaxLength(100)]
        public string? Description { get; set; }
        [Range(1,10)]
        public int MaxAdult { get; set; }
        [Range(0, 10)]
        public int MaxChildren { get; set; }
        public bool IsActive { get; set; }
        public RoomTypeSideOptionSaveModel[] RoomTypes { get; set; } = [];

        public class RoomTypeSideOptionSaveModel
        {
            public int Id { get; set; }
            public int? Unit { get; set; }
            public RoomTypeSideOptionSaveModel(int id, int? unit) => (Id, Unit) = (id, unit);
        }
        

    }
    
}
