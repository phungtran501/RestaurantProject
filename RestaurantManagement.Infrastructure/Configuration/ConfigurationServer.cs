using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RestaurantManagement.Data;
using RestaurantManagement.Data.Abstract;
using RestaurantManagement.Domain.Configuration;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Domain.Helper;
using RestaurantManagement.Service;
using RestaurantManagement.Service.Abstracts;

namespace RestaurantManagement.Infrastructure.Configuration
{
    public static class ConfigurationServer
    {
        public static void RegisterContextDb(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddDbContext<RestaurantManagementContext>(options => options
            .UseSqlServer(configuration.GetConnectionString("RestaurantDB"),
            options => options.MigrationsAssembly(typeof(RestaurantManagementContext).Assembly.FullName)));

            service.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequireNonAlphanumeric = true;
                config.Password.RequireDigit = true;
                config.Password.RequiredLength = 5;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<RestaurantManagementContext>();
        }

        //extension method
        //register dependency injection
        public static void RegisterDI(this IServiceCollection service, IConfiguration configuration)
        {
            service.Configure<EmailConfig>(configuration.GetSection("MailSettings"));

            service.AddTransient<PasswordHasher<ApplicationUser>>();
            service.AddTransient(typeof(IRepository<>), typeof(Repository<>));
            service.AddTransient<IUnitOfWork, UnitOfWork>();
            service.AddTransient<IDapperHelper, DapperHelper>();
            service.AddTransient<IImageHandler, ImageHandler>();
            service.AddTransient<IUserService, UserService>();
            service.AddTransient<IAccountService, AccountService>();
            service.AddTransient<IRoleService, RoleService>();
            service.AddTransient<ICategoriesService, CategoriesService>();
            service.AddTransient<IFoodService, FoodService>();
            service.AddTransient<IMenuService, MenuService>();
            service.AddTransient<IUserAddressService, UserAddressService>();
            service.AddTransient<ICommentService, CommentService>();
            service.AddTransient<IMenuDetailService, MenuDetailService>();
            service.AddTransient<IEmailHelper, EmailHelper>();
            service.AddTransient<ICartService, CartService>();
            



        }

        public static async Task SeedData(this WebApplication webApplication, IConfiguration configuration)
        {
            using (var scope = webApplication.Services.CreateScope())
            {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var passwordHasher = scope.ServiceProvider.GetRequiredService<PasswordHasher<ApplicationUser>>();

                var usersDefault =   configuration.GetSection("DefaultUsers")?.Get<List<DefaultUser>>();
                var rolesDefault = configuration.GetSection("DefaultRoles")?.Get<List<string>>();


                // Create new roles
                foreach (var role in rolesDefault)
                {
                    if (!(await roleManager.RoleExistsAsync(role)))
                    {
                        await roleManager.CreateAsync(new IdentityRole
                        {
                            Id = Guid.NewGuid().ToString(),
                            Name = role,
                            NormalizedName = role
                        });
                    }
                }

                // Create new users
                foreach (var user in usersDefault)
                {
                    var userExist = await userManager.FindByNameAsync(user.Username);

                    if (userExist is not null)
                    {
                        continue;
                    }

                    await userManager.CreateAsync(new ApplicationUser
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = user.Username,
                        PasswordHash = passwordHasher.HashPassword(new ApplicationUser(), user.Password),
                        EmailConfirmed = false,
                        PhoneNumberConfirmed = false,
                        TwoFactorEnabled = false,
                        LockoutEnabled = false,
                        AccessFailedCount = 0
                    });

                    var currentUser = await userManager.FindByNameAsync(user.Username);

                    await userManager.AddToRoleAsync(currentUser, user.Role);

                }
            }
        }
    }

    public class DefaultUser {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

}
