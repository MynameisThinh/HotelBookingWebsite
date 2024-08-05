using HotelBookingWebsite.Data;
using HotelBookingWebsite.Data.Entities;
using HotelBookingWebsite.Models;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingWebsite.Services
{
    public interface ISideOptionService
    {
        Task<SideOption[]> GetSideOptionsAsync();
        Task<MethodResult<SideOption>> SaveSideOptionAsync(SideOption sideOption);
        Task<MethodResult<bool>> DeleteSideOptionAsync(int id);
    }

    public class SideOptionService : ISideOptionService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory; //tạo instance của ApplicationDbContext
        public SideOptionService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory; //instructor
        }


        public async Task<SideOption[]> GetSideOptionsAsync()
        {
            using var context = _contextFactory.CreateDbContext(); //sử dụng using để cho mọi trường hợp
            return await context.SideOptions.Where(a => !a.IsDeleted).ToArrayAsync();
        }
        public async Task<MethodResult<bool>> DeleteSideOptionAsync(int id)
        {
            using var context = _contextFactory.CreateDbContext();
            var sideO = await context.SideOptions.AsTracking().FirstOrDefaultAsync(a => a.Id == id);
            if(sideO is not null)
            {
                sideO.IsDeleted = true;
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<MethodResult<SideOption>> SaveSideOptionAsync(SideOption sideOption)
        {
            using var context = _contextFactory.CreateDbContext();
            if (sideOption.Id == 0)
            {
                //Tạo một sideoption mới
                if(await context.SideOptions.AnyAsync(a => a.Name == sideOption.Name && !a.IsDeleted))
                {
                    //return MethodResult<SideOption>.Failure("SideOption đã tồn tại");
                    return "SideOption đã tồn tại";
                }
                await context.SideOptions.AddAsync(sideOption);
            }
            else
            {
                //Cập nhật sideoption mới
                if (await context.SideOptions.AnyAsync(a => a.Name == sideOption.Name && a.Id != sideOption.Id))
                {
                    //return MethodResult<SideOption>.Failure("SideOption đã tồn tại");
                    return "SideOption đã tồn tại vì cùng tên";
                }

                var dbSideOption = await context.SideOptions
                    .AsTracking()
                    .FirstOrDefaultAsync(a => a.Id == sideOption.Id)
                    ?? throw new InvalidOperationException("Side option không tồn tại");
                dbSideOption.Name = sideOption.Name;
                dbSideOption.Icon = sideOption.Icon;
                //context.SideOptions.Update(dbSideOption); //using tracking thay thế 
            }
            await context.SaveChangesAsync();
            return sideOption;
        }
    }
    public class RoomService
    {

    }
}
