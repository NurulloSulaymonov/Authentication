using Auth.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(connectionString));


builder.Services.AddIdentity<IdentityUser, IdentityRole>(config =>
    {
        config.Password.RequiredLength = 4;
    })
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthentication("Cookies")
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,option =>
    {
        option.LoginPath = "/account/login";
        option.AccessDeniedPath = "/account/accessDenied";
        option.Cookie.Name = "AuthCookie";

    });


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
