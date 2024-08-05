namespace HotelBookingWebsite.Extensions
{
    public static class StringExtension
    {
        public static string Eclipse(this string str, int maxLength) =>
            (string.IsNullOrWhiteSpace(str) || str.Length <= maxLength) ?
            str : $"{str[0..97]}...";
    }
}
