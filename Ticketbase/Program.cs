using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Ticketbase.Data;

var builder = WebApplication.CreateBuilder(args);

// Database connection
builder.Services.AddDbContext<TicketbaseContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TicketbaseContext")
        ?? throw new InvalidOperationException("Connection string 'TicketbaseContext' not found.")));

// Add MVC + API controllers
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(); // <-- add this so [ApiController] endpoints work

// Add CORS policy for React frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy => policy.WithOrigins("http://localhost:5173",   // Vite dev server
                                     "https://your-frontend.azurestaticapps.net") // deployed React app
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Cookie authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// User secrets
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddUserSecrets<Program>();
}

var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseCors("AllowFrontend"); // <-- enable CORS here

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

// MVC routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Info}/{id?}")
    .WithStaticAssets();

// API routes
app.MapControllers(); // <-- maps your new API controllers

app.Run();