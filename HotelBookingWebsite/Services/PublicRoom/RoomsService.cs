using HotelBookingWebsite.Data;
using HotelBookingWebsite.Data.Entities;
using HotelBookingWebsite.Models.Public;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingWebsite.Services.PublicRoom
{
    public interface IRoomsService
    {
        Task<RoomTypeModel[]> GetRoomTypesAsync(int count = 0,FilterModel? filter = null);
        Task<LookUpModel<short,decimal>[]> GetRoomTypesLookUp(FilterModel? filter = null);
    }

    public class RoomsService : IRoomsService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        public RoomsService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<RoomTypeModel[]> GetRoomTypesAsync(int count = 0, FilterModel? filter = null)
        {
            using var context = _contextFactory.CreateDbContext();
            var query = ApplyFilters(context.RoomTypes, filter);
            if (count > 0)
            {
                query = query.Take(count);
            }
            return await query.Select(a =>
                                  new RoomTypeModel(
                                      a.Id,
                                      a.Name,
                                      a.Image,
                                      a.Description,
                                      a.Price,
                                      a.SideOptions.Select(rt => new RoomTypeSideOptionModel(rt.SideOption.Name, rt.SideOption.Icon, rt.Unit))
                                      .ToArray())).ToArrayAsync();

        }
        public async Task<LookUpModel<short,decimal>[]> GetRoomTypesLookUp(FilterModel? filter = null)
        {
            using var context= _contextFactory.CreateDbContext();
            var query = ApplyFilters(context.RoomTypes, filter);
            return await query.Select(rt => new LookUpModel<short,decimal>(rt.Id, rt.Name!,rt.Price))
                                          .ToArrayAsync();
        } 

        private static IQueryable<RoomType> ApplyFilters(IQueryable<RoomType> q,FilterModel? filter = null)
        {
            var query = q.Where(a => a.IsActive);

            if (filter is not null)
            {
                //Lấy bookingId cho checkin checkout
                //kiem tra xem phong co san tuong ung voi nhung checkin checkout khong
                if (filter.Adults > 0)
                {
                    query = query.Where(a => a.MaxAdult >= filter.Adults);
                }
                if (filter.Children > 0)
                {
                    query = query.Where(a => a.MaxChildren >= filter.Children);
                }
            }
            return query;
        }
    }
}
