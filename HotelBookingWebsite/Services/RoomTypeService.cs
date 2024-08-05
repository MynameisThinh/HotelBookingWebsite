
using HotelBookingWebsite.Components.Pages;
using HotelBookingWebsite.Data;
using HotelBookingWebsite.Data.Entities;
using HotelBookingWebsite.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace HotelBookingWebsite.Services
{
    public interface IRoomTypeService
    {
        Task<MethodResult<short>> SaveRoomTypeAsync(RoomTypeSaveModel model, string userId);
        Task<RoomTypeListModel[]> GetRoomTypeForManagePageAsync();

        Task<RoomTypeSaveModel?> GetRoomTypeToEditAsync(short id);
        Task<Room[]> GetRoomsAsync(short roomTypeId);
        Task<MethodResult<Room>> SaveRoomsAsync(Room room);
        Task<MethodResult> DeleteRoomAsync(int roomId);

        Task<MethodResult> AssignRoomBookingAsync(long bookingId, int roomId);

        
    }

    public class RoomTypeService : IRoomTypeService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public RoomTypeService(IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<MethodResult<short>> SaveRoomTypeAsync(RoomTypeSaveModel model, string userId)
        {
            using var context = _contextFactory.CreateDbContext();
            RoomType? roomType;
            if (model.Id == 0)
            {
                if (await context.RoomTypes.AnyAsync(s => s.Name == model.Name))
                {
                    return $"Loại phòng đã tồn tại với cùng tên {model.Name}";
                }
                roomType = new RoomType
                {
                    Name = model.Name,
                    AddedBy = userId,
                    AddedOn = DateTime.Now,
                    Description = model.Description,
                    Image = model.Image,
                    IsActive = true ,//model.IsActive,
                    MaxAdult = model.MaxAdult,
                    MaxChildren = model.MaxChildren,
                    Price = model.Price,
                };

                await context.RoomTypes.AddAsync(roomType);
            }
            else
            {
                if (await context.RoomTypes.AnyAsync(rt => rt.Name == model.Name && rt.Id != model.Id))
                {
                    return $"Loại phòng đã tồn tại với cùng tên {model.Name}";
                }
                roomType = await context.RoomTypes.AsTracking().FirstOrDefaultAsync(rt => rt.Id == model.Id);
                if(roomType is null)
                {
                    return $"Menh lenh khong hop le";
                }

                roomType!.Name = model.Name;
                roomType.Description = model.Description;
                roomType.Image = model.Image;
                roomType.IsActive = true;//model.IsActive;
                roomType.MaxAdult = model.MaxAdult;
                roomType.MaxChildren = model.MaxChildren;
                roomType.Price = model.Price;


                roomType.LastUpdatedBy = userId;
                roomType.LastUpdatedOn = DateTime.Now;

                var existingRoomTypeSideOptions = await context.RoomTypeSideOptions.Where(a => a.RoomTypeId == model.Id).ToListAsync(); 
                context.RoomTypeSideOptions.RemoveRange(existingRoomTypeSideOptions);
            }
            
            await context.SaveChangesAsync();
            if (model.RoomTypes.Length > 0)
            {
                var roomTypeSO = model.RoomTypes.Select(a => new RoomTypeSideOption
                {
                    SideOptionId = a.Id,
                    RoomTypeId = roomType.Id,
                    Unit = a.Unit,
                });

                await context.RoomTypeSideOptions.AddRangeAsync(roomTypeSO);
                await context.SaveChangesAsync();
            }
            return roomType.Id;
        }

        public async Task<RoomTypeListModel[]> GetRoomTypeForManagePageAsync()
        {
            using var context = _contextFactory.CreateDbContext();
            return await context.RoomTypes.Select(rt => new RoomTypeListModel(rt.Id,rt.Name!,rt.Image!,rt.Price)).ToArrayAsync();
        }

        public async Task<RoomTypeSaveModel?> GetRoomTypeToEditAsync(short id)
        {
            using var context = _contextFactory.CreateDbContext();
            var roomType = await context.RoomTypes
                                        .Include(rt => rt.SideOptions)
                                        .Where(rt => rt.Id == id)
                                        .Select(rt => new RoomTypeSaveModel
                                        {
                                            Name = rt.Name,
                                            Image = rt.Image,
                                            Price = rt.Price,
                                            Description = rt.Description,
                                            IsActive = rt.IsActive,
                                            Id = id,
                                            MaxAdult = rt.MaxAdult,
                                            MaxChildren = rt.MaxChildren,
                                            RoomTypes = rt.SideOptions.Select(a =>new RoomTypeSaveModel.RoomTypeSideOptionSaveModel(a.SideOptionId, a.Unit)).ToArray(),
                                        }).FirstOrDefaultAsync();
            return roomType;
                                        
        }

        public async Task<Room[]> GetRoomsAsync(short roomTypeId)
        {
            using var _context = _contextFactory.CreateDbContext();
            return await _context.Rooms.Where(r => r.RoomTypeId == roomTypeId && !r.IsDeleted)
                                       .ToArrayAsync();
        }
        public async Task<MethodResult<Room>> SaveRoomsAsync(Room room)
        {
            try
            {
                using var context = _contextFactory.CreateDbContext();
                if (room.ID == 0)
                {
                    if(await context.Rooms.AnyAsync(r => r.RoomNumber == room.RoomNumber))
                    {
                        return "Số phòng đã tồn tại";
                    }
                    await context.Rooms.AddAsync(room);

                }
                else
                {
                    var dbRoom = await context.Rooms.AsTracking().FirstOrDefaultAsync(x => x.ID == room.ID && !x.IsDeleted);
                    if (dbRoom is null)
                    {
                        return "Khong hop le";
                    }
                    dbRoom.IsAvailable = room.IsAvailable;
                }
                await context.SaveChangesAsync();
            }catch(Exception ex)
            {
                return ex.InnerException ?.Message ?? ex.Message;
            }
            return room;
        }

        public async Task<MethodResult> DeleteRoomAsync(int roomId)
        {
            using var context = _contextFactory.CreateDbContext(); 
            var dbRoom = await context.Rooms.AsTracking().FirstOrDefaultAsync(r => r.ID ==  roomId);
            if (dbRoom is null)
            {
                return "Khong hop le";
            }
            dbRoom.IsDeleted = true;
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<MethodResult> AssignRoomBookingAsync(long bookingId,  int roomId)
        {
            using var context = _contextFactory.CreateDbContext();
            var room = await context.Rooms
                                    .AsTracking()
                                    .FirstOrDefaultAsync(r => r.ID == roomId && !r.IsDeleted);
            if (room is null)
                return "Không hợp lệ";
            if (!room.IsAvailable)
                return "Không còn phòng";
            var booking = await context.Bookings.AsTracking().FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking is null)
                return "Không hợp lệ";
            if (booking.RoomId.HasValue)
            {
                var existingRoom = await context.Rooms.AsTracking().FirstOrDefaultAsync(b => b.ID == booking.RoomId.Value);
                if(existingRoom is not null)
                {
                    existingRoom.IsAvailable = true;
                }
            }
            room.IsAvailable = false;
            
            booking.RoomId = roomId;
            
            await context.SaveChangesAsync();
            return true;
        }

        
    }
}
