using HotelBookingWebsite.Data.Entities;
using HotelBookingWebsite.Data;
using HotelBookingWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingWebsite.Services
{
    public interface ISubscriberService
    {
        Task<MethodResult> AddSubscribeAsync(string email);
        Task<PagedResult<Subscriber>> GetSubscribeAsync(int startIndex, int pageSizes);
    }

    public class SubscriberService : ISubscriberService
    {
        private IDbContextFactory<ApplicationDbContext> _contextFactory;

        public SubscriberService(IDbContextFactory<ApplicationDbContext> contextFactory) => _contextFactory = contextFactory;

        public async Task<MethodResult> AddSubscribeAsync(string email)
        {
            using var context = _contextFactory.CreateDbContext();
            var subscriber = new Subscriber
            {
                Email = email,
                SubscribeOn = DateTime.Now,
            };
            await context.Subscribers.AddAsync(subscriber);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<PagedResult<Subscriber>> GetSubscribeAsync(int startIndex, int pageSizes)
        {
            using var context = _contextFactory.CreateDbContext();
            var totalCount = await context.Subscribers.CountAsync();

            var record = await context.Subscribers.OrderByDescending(b => b.SubscribeOn)
                                                .Skip(startIndex)
                                                .Take(pageSizes)
                                                .ToArrayAsync();
            return new PagedResult<Subscriber>(totalCount, record);
        }
    }
}
