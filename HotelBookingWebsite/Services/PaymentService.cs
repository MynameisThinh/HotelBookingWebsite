using HotelBookingWebsite.Data;
using HotelBookingWebsite.Data.Entities;
using HotelBookingWebsite.Models;
using HotelBookingWebsite.Models.Public;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Stripe.Checkout;

namespace HotelBookingWebsite.Services
{
    public interface IPaymentService
    {
        Task<string> GeneratePaymentUrl(PaymentModel model, string userId, string domain);
        
        Task<MethodResult<string?>> ConfirmPaymentAsync(string paymentIdStr, long bookingId, string checkoutSessionId);
        Task<MethodResult> CancelPaymentAsync(string paymentIdStr, long bookingId, string checkoutSessionId);
    }

    public class PaymentService : IPaymentService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserManager<ApplicationUser> userManager;
        private const string StripePaymentInitated = "initated";
        private const string StripePaymentSuccess = "paid";
        private const string StripePaymentFailed = "unpaid";

        public PaymentService(IDbContextFactory<ApplicationDbContext> contextFactory,UserManager<ApplicationUser> userManager)
        {
            _contextFactory = contextFactory;
            this.userManager = userManager;
        }
        //Ap dung stripe payment cho web
        public async Task<string> GeneratePaymentUrl(PaymentModel model, string userId, string domain)
        {
            using var context = _contextFactory.CreateDbContext();
            var paymentEntity = new Payment
            {
                BookingId = model.BookingId,
                CreatedAt = DateTime.Now,
                Status = StripePaymentInitated,
                CheckoutSessionId = "Pending"
            };
            await context.Payments.AddAsync(paymentEntity);
            await context.SaveChangesAsync();
            var lineItems = new SessionLineItemOptions
            {
                Quantity = 1,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "VND",
                    UnitAmountDecimal = model.TotalAmount * 25000,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = model.roomTypeName,
                        Description = $"Đặt phòng {model.roomTypeName} trong {model.NumOfDays} ngày",

                    }
                }
            };
            var sessionCreateOptions = new SessionCreateOptions
            {
                LineItems = [lineItems],
                Mode = "payment",
                SuccessUrl =$"{domain}/bookings/{model.BookingId}" + "/success?session-id={CHECKOUT_SESSION_ID}&payment-id=" + paymentEntity.Id,
                CancelUrl = $"{domain}/bookings/{model.BookingId}/cancel?session-id={{CHECKOUT_SESSION_ID}}&payment-id={paymentEntity.Id}",
            };
            var sessionService = new SessionService();
            var session = await sessionService.CreateAsync(sessionCreateOptions);

            paymentEntity.CheckoutSessionId = session.Id;
            await context.SaveChangesAsync();
            return session.Url;
        }
        public async Task<MethodResult<string?>> ConfirmPaymentAsync(string paymentIdStr,long bookingId, string checkoutSessionId)
        {

            
            using var context = _contextFactory.CreateDbContext();
            var paymentEtt = await context.Payments
                                          .AsTracking()
                                          .FirstOrDefaultAsync(p => p.Id == Guid.Parse(paymentIdStr) && p.CheckoutSessionId == checkoutSessionId);
            if (paymentEtt is null)
            {
                return new MethodResult<string?>(false, "paymentId không hợp lệ", default);
            }
            var booking = await context.Bookings
                                           .AsTracking()
                                           .FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking is null)
            {
                return new MethodResult<string?>(false, "paymentId không hợp lệ", default);
            }
            if (paymentEtt.Status != StripePaymentInitated)
            {
                //Da co don thanh toan nay roi

            }
            else
            {
                var sessionService = new SessionService();
                var checkoutSession = await sessionService.GetAsync(checkoutSessionId);
                if (checkoutSession is null)
                {
                    return new MethodResult<string?>(false, "CheckoutSession không hợp lệ", default);
                }


                paymentEtt.Status = checkoutSession.PaymentStatus;
                paymentEtt.AdditionalInfo = $"Tên : {checkoutSession.CustomerDetails.Name} Email :{checkoutSession.CustomerDetails.Email}";
                

                booking.Status = checkoutSession.PaymentStatus == StripePaymentSuccess ?
                                 Constants.BookingStatus.PaymentSuccess :
                                 Constants.BookingStatus.PaymentCancelled;
                await context.SaveChangesAsync();
            }
            
           
            var guestName = await userManager.Users
                                  .Where(u => u.Id == booking.GuestId)
                                  .Select(u => u.FullName)
                                  .FirstOrDefaultAsync();
            return new MethodResult<string?>(true,null,guestName);
        }

        public async Task<MethodResult> CancelPaymentAsync(string paymentIdStr, long bookingId, string checkoutSessionId)
        {
            using var context = _contextFactory.CreateDbContext();
            var paymentEtt = await context.Payments
                                          .AsTracking()
                                          .FirstOrDefaultAsync(p => p.Id == Guid.Parse(paymentIdStr) && p.CheckoutSessionId == checkoutSessionId);
            if (paymentEtt is null)
            {
                return new MethodResult(false, "paymentId không hợp lệ");
            }
            var booking = await context.Bookings
                                           .AsTracking()
                                           .FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking is null)
            {
                return new MethodResult(false, "paymentId không hợp lệ");
            }
            if (paymentEtt.Status == StripePaymentInitated)
            {
                var sessionService = new SessionService();
                var checkoutSession = await sessionService.GetAsync(checkoutSessionId);
                if (checkoutSession is null)
                {
                    return new MethodResult(false, "CheckoutSession không hợp lệ");
                }


                paymentEtt.Status = "Cancelled";
                paymentEtt.AdditionalInfo = $"Hủy bởi khách ";


                booking.Status = Constants.BookingStatus.PaymentCancelled;

;                await context.SaveChangesAsync();
            }



            return true;
        }
    }
}
