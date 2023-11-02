using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagement.Data;
using RestaurantManagement.Domain.Entities;
using RestaurantManagement.Infrastructure.Configuration;
using RestaurantManagement.UI.Helper;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("RestaurantDB") ?? throw new InvalidOperationException("Connection string 'RestaurantManagementContextConnection' not found.");


var buiderRazor = builder.Services.AddRazorPages();

builder.Services.AddRazorPages();
// Add services to the container.
builder.Services.RegisterContextDb(builder.Configuration);

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


builder.Services.AddControllersWithViews();



builder.Services.RegisterDI();

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(2);

    options.LoginPath = "/admin/authentication/login";
    //options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;

    options.Cookie.Name = "RestaurantCookie";
});

builder.Services.AddSession(option =>
{
    option.IdleTimeout = TimeSpan.FromHours(2);
});

var app = builder.Build();

app.SeedData(builder.Configuration).GetAwaiter().GetResult();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    buiderRazor.AddRazorRuntimeCompilation();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();

var timeOutCacheStaticFiles = 60 * 60;

app.UseStaticFiles( new StaticFileOptions
{
    OnPrepareResponse = cg =>
    {
        cg.Context.Response.Headers.Append("Cache-Control", $"public, max-age={timeOutCacheStaticFiles}");
    }
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.MapControllerRoute(
    name: "System",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//app.MapAreaControllerRoute(
//    name: "SystemRouting",
//    areaName: "System",
//    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "FoodByCategoryById",
    pattern: "food/get-by-category/{id?}",
    defaults: new { controller = "Food", action = "GetFoodByCategoryId" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
