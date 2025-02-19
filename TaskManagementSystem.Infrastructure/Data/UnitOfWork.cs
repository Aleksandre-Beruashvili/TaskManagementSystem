using System.Threading.Tasks;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Interfaces;
using TaskManagementSystem.Infrastructure.Repositories;

namespace TaskManagementSystem.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private Repository<Project> _projectRepository;
        private Repository<TaskItem> _taskRepository;
        private Repository<Label> _labelRepository;
        private Repository<ActivityLog> _activityLogRepository;
        private Repository<Team> _teamRepository; // NEW

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IRepository<Project> ProjectRepository => _projectRepository ??= new Repository<Project>(_context);
        public IRepository<TaskItem> TaskRepository => _taskRepository ??= new Repository<TaskItem>(_context);
        public IRepository<Label> LabelRepository => _labelRepository ??= new Repository<Label>(_context);
        public IRepository<ActivityLog> ActivityLogRepository => _activityLogRepository ??= new Repository<ActivityLog>(_context);
        public IRepository<Team> TeamRepository => _teamRepository ??= new Repository<Team>(_context); // NEW

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
