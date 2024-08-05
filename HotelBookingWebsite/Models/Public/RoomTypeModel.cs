using HotelBookingWebsite.Data.Entities;

namespace HotelBookingWebsite.Models.Public
{
    public readonly record struct RoomTypeSideOptionModel(string Name,string? Icon = null,int? Unit = null);
    public record RoomTypeModel(short Id,string Name,string Image, string Description,decimal Price, RoomTypeSideOptionModel[] SideOptions);
}
