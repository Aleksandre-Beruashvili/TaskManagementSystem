using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Infrastructure.Identity;

namespace TaskManagementSystem.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Project> Projects { get; set; }
        public DbSet<TaskItem> TaskItems { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<ActivityLog> ActivityLogs { get; set; }
        public DbSet<Team> Teams { get; set; } // NEW

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("dotnet");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        }
    }
}
