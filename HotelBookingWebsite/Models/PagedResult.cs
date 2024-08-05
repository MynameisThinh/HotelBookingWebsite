namespace HotelBookingWebsite.Models
{
    public record PagedResult<TData>(int totalCount, TData[] Records);
}
