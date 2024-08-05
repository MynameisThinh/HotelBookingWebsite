using HotelBookingWebsite.Data;
using HotelBookingWebsite.Data.Entities;
using HotelBookingWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingWebsite.Services
{
    public interface IEnquiryService
    {
        Task<MethodResult> AddEnquiryAsync(EnquiryModel model);
        Task<PagedResult<Enquiry>> GetEnquiriesAsync(int startIndex, int pageSizes);
    }

    public class EnquiryService : IEnquiryService
    {
        private IDbContextFactory<ApplicationDbContext> _contextFactory;

        public EnquiryService(IDbContextFactory<ApplicationDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<MethodResult> AddEnquiryAsync(EnquiryModel model)
        {
            using var context = _contextFactory.CreateDbContext();
            var enquiry = new Enquiry
            {
                Email = model.Email,
                Message = model.Message,
                Name = model.Name,
                Subject = model.Subject,
                EnquireOn = DateTime.Now,
            };
            await context.Enquiries.AddAsync(enquiry);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<Enquiry>> GetEnquiriesAsync(int startIndex, int pageSizes)
        {
            using var context = _contextFactory.CreateDbContext();
            var totalCount = await context.Enquiries.CountAsync();

            var record = await context.Enquiries.OrderByDescending(b => b.EnquireOn)
                                                .Skip(startIndex)
                                                .Take(pageSizes)
                                                .ToArrayAsync();
            return new PagedResult<Enquiry>(totalCount, record);
        }
    }
}
