using HotelBookingWebsite.Constants;
using HotelBookingWebsite.Data;
using HotelBookingWebsite.Data.Entities;
using HotelBookingWebsite.Models;
using HotelBookingWebsite.Models.Public;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace HotelBookingWebsite.Services
{
    public interface IBookingService
    {
        Task<MethodResult<long>> MakeBookingAsync(BookingModel bookingModel, string userId);
        Task<PagedResult<BookingDisplayModel>> GetBookingAsync(int startIndex, int pageSize);

        Task<MethodResult> ApproveBookingAsync(long BookingId);

        Task<MethodResult> CancelBookingAsync(long BookingId,string cancelReason, string? userId = null);

        Task<PagedResult<BookingDisplayModel>> GetBookingForGuestAsync(string? guestId, BookingDisplayType type, int startIndex, int pageSize);
    }

    public class BookingService : IBookingService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public BookingService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<MethodResult<long>> MakeBookingAsync(BookingModel bookingModel, string userId)
        {
            try
            {
                var booking = new Booking
                {
                    Adult = bookingModel.Adults ?? 0,
                    BookedOn = DateTime.Now,
                    CheckInDate = bookingModel.CheckInDate,
                    CheckOutDate = bookingModel.CheckOutDate,
                    Children = bookingModel.Children ?? 0,
                    GuestId = userId,
                    RoomTypeId = bookingModel.RoomTypeId,
                    SpecialRequest = bookingModel.SpecialRequest,
                    Status = Constants.BookingStatus.Pending,
                    TotalAmount = bookingModel.Amount,
                };
                using var context = _contextFactory.CreateDbContext();
                await context.Bookings.AddAsync(booking);
                await context.SaveChangesAsync();
                return booking.Id;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public async Task<PagedResult<BookingDisplayModel>> GetBookingAsync(int startIndex,int pageSize)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = context.Bookings;
            var totalCount = await query.CountAsync();
            var bookings = await query.OrderByDescending(x => x.CheckInDate)
                .Select(BookingDisplayModelSelector)
                                     .Skip(startIndex)
                                     .Take(pageSize)
                                     .ToArrayAsync();

            return new PagedResult<BookingDisplayModel>(totalCount, bookings);
        }

        public async Task<MethodResult> ApproveBookingAsync(long BookingId)
        {
            using var context = _contextFactory.CreateDbContext();
            var booking = await context.Bookings
                                      .AsTracking()
                                      .FirstOrDefaultAsync(b => b.Id == BookingId);
            if (booking is null)
                return "Yêu cầu không hợp lệ";

            switch (booking.Status)
            {
                case Constants.BookingStatus.Booked:
                    return "Đã đặt phòng";
                case Constants.BookingStatus.Cancelled:
                    return "Hủy đặt phòng";
                case Constants.BookingStatus.PaymentSuccess:
                    booking.Status = Constants.BookingStatus.Booked;
                    break;

                default:
                    return "Đặt phòng chỉ có hiệu lực sau khi thanh toán";
            }
            await context.SaveChangesAsync();
            return true;

        }

        public async Task<MethodResult> CancelBookingAsync(long BookingId, string cancelReason,string? userId = null)
        {
            using var context = _contextFactory.CreateDbContext();
            var booking = await context.Bookings
                                      .AsTracking()
                                      .FirstOrDefaultAsync(b => b.Id == BookingId);
            if (booking is null)
                return "Yêu cầu không hợp lệ";

            if(booking.Status == Constants.BookingStatus.Cancelled)
                return "Đã hủy đặt phòng";

            booking.Status = Constants.BookingStatus.Cancelled;   
            booking.Remarks += Environment.NewLine + $" Hủy bởi {(userId == booking.GuestId ? "Khách" : "Staff/AdminLayout")}. Lý do :{cancelReason}";
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<BookingDisplayModel>> GetBookingForGuestAsync(string? guestId, BookingDisplayType type, int startIndex, int pageSize)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = context.Bookings.Where(b => b.GuestId == guestId);

            var now = DateOnly.FromDateTime(DateTime.Now);
            query = type switch
            {
                BookingDisplayType.Upcoming => query.Where(b => b.CheckInDate > now),
                BookingDisplayType.Ongoing => query.Where(b => b.CheckInDate == now || b.CheckOutDate == now),
                BookingDisplayType.Past => query.Where(b => b.CheckInDate < now),
            };
            
            var totalCount = await query.CountAsync();
            var bookings = await query.OrderByDescending(x => x.CheckInDate)
                .Select(BookingDisplayModelSelector)
                                     .Skip(startIndex)
                                     .Take(pageSize)
                                     .ToArrayAsync();

            return new PagedResult<BookingDisplayModel>(totalCount, bookings);
        }

        private static Expression<Func<Booking, BookingDisplayModel>> BookingDisplayModelSelector = 
            b => new BookingDisplayModel
            {
                Adult = b.Adult,
                BookedOn = b.BookedOn,
                CheckInDate = b.CheckInDate,
                CheckOutDate = b.CheckOutDate,
                Children = b.Children,
                GuestId = b.GuestId,
                GuestName = b.Guest.FullName,
                RoomTypeId = b.RoomTypeId,
                RoomTypeName = b.RoomType.Name,
                Id = b.Id,
                RoomId = b.RoomId,
                RoomNumber = b.RoomId == null ? "" : b.Room.RoomNumber,
                SpecialRequest = b.SpecialRequest,
                Status = b.Status,
                TotalAmount = b.TotalAmount,
                Remarks = b.Remarks
            };

    }
}
