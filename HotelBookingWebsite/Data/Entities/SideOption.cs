using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingWebsite.Data.Entities
{
    public class SideOption
    {
        [Key]
        public int Id { get; set; }
        [Required, MaxLength(25)]
        public string Name { get; set; }
        [Required, MaxLength(25),Unicode(false)]
        public string Icon { get; set; }
        public bool IsDeleted { get;set; }
        public SideOption Clone() => (MemberwiseClone() as SideOption)!;
    }
}
