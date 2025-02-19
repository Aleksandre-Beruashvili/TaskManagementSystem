using System.Threading.Tasks;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Domain.Interfaces
{
    public interface IUnitOfWork
    {
        IRepository<Project> ProjectRepository { get; }
        IRepository<TaskItem> TaskRepository { get; }
        IRepository<Label> LabelRepository { get; }
        IRepository<ActivityLog> ActivityLogRepository { get; }
        IRepository<Team> TeamRepository { get; } // NEW
        Task<int> CommitAsync();
    }
}
