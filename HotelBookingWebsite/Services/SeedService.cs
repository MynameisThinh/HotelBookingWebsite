
using HotelBookingWebsite.Constants;
using HotelBookingWebsite.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace HotelBookingWebsite.Services
{
    public class SeedService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IDbContextFactory<ApplicationDbContext> _contextFactory;

        public SeedService(UserManager<ApplicationUser> userManager,IUserStore<ApplicationUser> userStore,
                           RoleManager<IdentityRole> roleManager,IConfiguration configuration,
                           IDbContextFactory<ApplicationDbContext> contextFactory)
        {
            _userManager = userManager;
            _userStore = userStore;
            _roleManager = roleManager;
            _configuration = configuration;
            _contextFactory = contextFactory;
        }
        public async Task SeedDatabaseSync()
        {
            using (var context = _contextFactory.CreateDbContext())
            {
                if(context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }
            var adminUserEmail = _configuration.GetValue<string>("AdminUser:Email");//tạo admin
            var dbAdminUser = await _userManager.FindByEmailAsync(adminUserEmail!);
            if (dbAdminUser is not null)
            {
                return;
            }
            var applicationUser = new ApplicationUser() //Tạo applicationuser cho admin để có thông tin đăng nhập
            {
                FirstName = _configuration.GetValue<string>("AdminUser:FirstName")!, //mock tất cả từ file appsettings.json
                LastName = _configuration.GetValue<string>("AdminUser:LastName"),
                RoleName = RoleType.Admin.ToString(),
                ContactNumber = _configuration.GetValue<string>("AdminUser:ContactNumber")!,
                Designation = "Administrator",
                
            }; 
            await _userStore.SetUserNameAsync(applicationUser,adminUserEmail,default); //set adminuser
            var emailStore = (IUserEmailStore<ApplicationUser>)_userStore; 
            await emailStore.SetEmailAsync(applicationUser,adminUserEmail,default);// set email cho user
            var result = await _userManager.CreateAsync(applicationUser, _configuration.GetValue<string>("AdminUser:Password")!); //tao password

            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
                throw new Exception($"Lỗi khi tạo tài khoản :{errors}");
                
            }
            if(await _roleManager.FindByNameAsync(RoleType.Admin.ToString()) is null) //nếu role trong csdl bị null
            {
                foreach(var roleName in Enum.GetNames<RoleType>())
                {
                    var role = new IdentityRole()
                    {
                        Name = roleName,
                    };
                    await _roleManager.CreateAsync(role); //tạo role mới => 3 role

                }
            }
            result = await _userManager.AddToRoleAsync(applicationUser, RoleType.Admin.ToString());//thêm role vào csdl
            if (!result.Succeeded)
            {
                var errors = string.Join(Environment.NewLine, result.Errors.Select(e => e.Description));
                throw new Exception($"Lỗi khi thêm tài khoản vào role Admin:{errors}");

            }
        }
    }
    
}
