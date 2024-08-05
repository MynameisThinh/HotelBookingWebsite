using HotelBookingWebsite.Constants;
using HotelBookingWebsite.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HotelBookingWebsite.Models.Public
{
    public class BookingModel
    {
        [Required, MaxLength(15)]
        public string? FirstName { get; set; } = "";
        [MaxLength(15)]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required, MaxLength(15), RegularExpression(@"^[0-9\+\(\)\s]+$")]
        public string? ContactNumber { get; set; } = "";
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = "";

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = "";


        public DateOnly CheckInDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public DateOnly CheckOutDate { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        public int? Adults { get; set; } = 0;
        public int? Children { get; set; } = 0;

        [Range(1,100,ErrorMessage ="Vui lòng chọn phòng")]
        public short RoomTypeId { get; set; }

        public decimal Amount { get;set; }

        [MaxLength(150)]
        public string? SpecialRequest { get; set; }
        public void SetCloneValues()
        {
            Email = "aolamtruongthinh@gmail.com";
            ContactNumber = "0123456789";
            FirstName = "Ao";
            LastName = "Thinh";
            Password = ConfirmPassword = "@Pass123";
        }
        //public void SetValueFromClaimsPrincipal(ClaimsPrincipal principal)
        //{
        //    if(principal?.Identity?.IsAuthenticated == true)
        //    {
        //        //var userId = principal.GetUserId();
        //        //var roleName = principal.FindFirstValue(AppConstants.CustomClaimTypes.RoleName);
        //        var fullName = principal.FindFirstValue(AppConstants.CustomClaimTypes.FullName);
        //        Email = principal.FindFirstValue(AppConstants.CustomClaimTypes.Email)!;
        //        ContactNumber = principal.FindFirstValue(AppConstants.CustomClaimTypes.ContactNumber);

        //        var parts = fullName.Split(' ');
        //        FirstName = parts[0];
        //        LastName = parts.Length > 1 ? parts[1] : null;
        //        Password = ConfirmPassword = "@Pass123";//(chỉ dùng để pass bypass trong DataAnnontation validation)
        //    }


        //}
    }
}
