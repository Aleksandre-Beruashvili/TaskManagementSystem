using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Infrastructure.Data;
using TaskManagementSystem.Infrastructure.Identity;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Infrastructure.Services;
using TaskManagementSystem.Infrastructure.Repositories;
using TaskManagementSystem.Domain.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Get connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure EF Core
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure cookie policy to enforce secure cookies and SameSite mode
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.Strict;
    options.Secure = Microsoft.AspNetCore.Http.CookieSecurePolicy.Always;
});

// Register Application and Infrastructure services
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ILabelService, LabelService>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

var app = builder.Build();

// Enforce HTTPS redirection
app.UseHttpsRedirection();

// Use cookie policy middleware to enforce cookie settings
app.UseCookiePolicy();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
