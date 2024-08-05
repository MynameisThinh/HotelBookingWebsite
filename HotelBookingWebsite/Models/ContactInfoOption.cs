namespace HotelBookingWebsite.Models
{
    public class ContactInfoOption
    {
        public const string Key = "ContactInfo"; // key từ appsettings.json
        public string ContactNumber { get; set; }

        public string Address { get; set; }

        public string GeneralEmail { get; set; }

        public string TechnicalEmail { get; set; }

        public string BookingEmail { get; set; }
    }
}
