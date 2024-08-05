using HotelBookingWebsite.Constants;
using HotelBookingWebsite.Data;
using HotelBookingWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingWebsite.Services
{
    public interface IUserService
    {
        Task<MethodResult<ApplicationUser>> CreateUserAsync(ApplicationUser user, string email, string password);
        Task<PagedResult<UserDisplayModel>> GetUsersAsync(int startIndex, int pageSize, RoleType? roleType = null);
        Task<MyProfileModel> GetProfileDetailsAsync(string userId);
        Task<MethodResult> UpdateProfileAsync(MyProfileModel model,RoleType? roleType = null);
        Task<MethodResult> ChangePasswordAsync(ChangePassModel model, string userId);
    }

    public class UserService : IUserService
    {
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;

        public UserService(IDbContextFactory<ApplicationDbContext> contextFactory,
            UserManager<ApplicationUser> userManager, IUserStore<ApplicationUser> userStore)
        {
            _contextFactory = contextFactory;
            _userManager = userManager;
            _userStore = userStore;
        }

        public async Task<PagedResult<UserDisplayModel>> GetUsersAsync(int startIndex, int pageSize, RoleType? roleType = null)
        {

            var query = _userManager.Users;
            if(roleType is not null)
            {
                query = query.Where(u => u.RoleName == roleType.ToString());

            }
            var total = await query.CountAsync();

            var records = await query.Select(u => new UserDisplayModel(u.Id,u.FullName,u.Email,u.RoleName,u.ContactNumber,u.Designation))
                              .Skip(startIndex)
                              .Take(pageSize)
                              .ToArrayAsync();
            return new PagedResult<UserDisplayModel>(total, records);
        }

        public async Task<MethodResult<ApplicationUser>> CreateUserAsync(ApplicationUser user, string email, string password)
        {

            var existingUser = await _userManager.FindByEmailAsync(email);
            if(existingUser is not null)
            {
                return new MethodResult<ApplicationUser>(false, $"Email đã tồn tại",existingUser);

            }
            await _userStore.SetUserNameAsync(user, email, CancellationToken.None);
            var emailStore = GetEmailStore();
            await emailStore.SetEmailAsync(user, email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                return $"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}";
            }

            result = await _userManager.AddToRoleAsync(user, user.RoleName ?? RoleType.Guest.ToString());
            if (!result.Succeeded)
            {
                return $"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}";

            }
            return user;
        }
        public async Task<MyProfileModel?> GetProfileDetailsAsync(string userId) =>
            await GetUser(userId)
                .Select(u => new MyProfileModel
                {
                    ContactNumber = u.ContactNumber,
                    Designation = u.Designation,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Id = u.Id,
                })
                .FirstOrDefaultAsync();

        private IQueryable<ApplicationUser> GetUser(string userId,RoleType? roleType = null)
        {
            var query =_userManager.Users.Where(u => u.Id == userId);
            if(roleType is not null)
            {
                query = query.Where(u => u.RoleName == roleType.ToString());

            }
            return query;
        }
           

        public async Task<MethodResult> UpdateProfileAsync(MyProfileModel model, RoleType? roleType = null)
        {
            var dbUser = await GetUser(model.Id,roleType)
                              .FirstOrDefaultAsync();
            if(dbUser is null)
                return "Yêu cầu không hợp lệ";
            dbUser.FirstName = model.FirstName;
            dbUser.LastName = model.LastName;
            dbUser.ContactNumber = model.ContactNumber;
            dbUser.Designation  = model.Designation;
            dbUser.Email = model.Email;
            var result = await _userManager.UpdateAsync(dbUser);
            if (!result.Succeeded)
            {
                return $"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}";
            }
            if(dbUser.Email.Equals(dbUser.Email,StringComparison.OrdinalIgnoreCase)) {
                //Admin không thay đổi email id 
                return true;
            }
            //Admin có thay đổi email id
            var changeToken = await _userManager.GenerateChangeEmailTokenAsync(dbUser,model.Email);
            result = await _userManager.ChangeEmailAsync(dbUser , model.Email, changeToken);
            if (!result.Succeeded)
            {
                return $"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}";
            }
            return true;
        }

        private IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }

        public async Task<MethodResult> ChangePasswordAsync(ChangePassModel model,string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if(user is null)
            {
                return "Không hợp lệ";
            }
            var result = await _userManager.ChangePasswordAsync(user,model.CurrentPassword,model.NewPassword);
            if (!result.Succeeded)
            {
                return $"Error: {string.Join(", ", result.Errors.Select(error => error.Description))}";
            }
            return true;
        }
    }
}
