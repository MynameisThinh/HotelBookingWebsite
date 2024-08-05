using HotelBookingWebsite.Data.Entities;

namespace HotelBookingWebsite.Data.Entities
{
    public class RoomTypeSideOption
    {

        public short RoomTypeId { get; set; }
        public int SideOptionId { get; set; }
        public int? Unit { get; set; }

        public virtual RoomType RoomType { get; set; }
        public virtual SideOption SideOption { get; set; }
    }
}
