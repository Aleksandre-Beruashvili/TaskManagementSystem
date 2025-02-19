using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Infrastructure.Data;

namespace TaskManagementSystem.Infrastructure.Identity
{
    public static class IdentityConfiguration
    {
        public static IServiceCollection AddCustomIdentity(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            return services;
        }
    }
}
