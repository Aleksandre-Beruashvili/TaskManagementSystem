using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TaskManagementSystem.Infrastructure.Data;
using TaskManagementSystem.Infrastructure.Identity;
using TaskManagementSystem.Infrastructure.Services;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Application.Services;
using TaskManagementSystem.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Retrieve connection string from configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Retrieve JWT settings from configuration (if available)
var jwtSection = builder.Configuration.GetSection("JwtSettings");
string secretKey = jwtSection["SecretKey"];

// Generate a secure 256-bit key if one is not provided (WARNING: This will change on every restart)
if (string.IsNullOrEmpty(secretKey))
{
    byte[] keyBytes = RandomNumberGenerator.GetBytes(32); // 32 bytes = 256 bits
    secretKey = Convert.ToBase64String(keyBytes);
    // Optionally, log or persist this key for consistency.
}

// Get Issuer and Audience (default values provided if not in config)
string issuer = jwtSection["Issuer"] ?? "YourAppIssuer";
string audience = jwtSection["Audience"] ?? "YourAppAudience";

// Configure EF Core with SQL Server
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configure ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = issuer,
        ValidAudience = audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
    };
});

// Register Unit of Work, Repositories, and Application Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ILabelService, LabelService>();
builder.Services.AddScoped<IActivityLogService, ActivityLogService>(); // For activity logs
builder.Services.AddScoped<ITeamService, TeamService>();               // For team collaboration

// Register Infrastructure External Services (Email and File Storage)
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IFileStorageService, FileStorageService>();

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
