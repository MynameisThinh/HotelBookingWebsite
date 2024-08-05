namespace HotelBookingWebsite.Models.Public
{

    public record PaymentModel(long BookingId,string roomTypeName,int NumOfDays, decimal TotalAmount);
}
