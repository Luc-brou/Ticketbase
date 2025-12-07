using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ticketbase.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TicketbaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TicketbaseContext")
        ?? throw new InvalidOperationException("Connection string 'TicketbaseContext' not found.")));

builder.Services.AddControllersWithViews();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173",
                                     "https://your-frontend.azurestaticapps.net")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowFrontend");

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Info}/{id?}")
    .WithStaticAssets();

app.MapControllers();

app.Run();