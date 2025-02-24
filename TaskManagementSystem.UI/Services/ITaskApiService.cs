using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.UI.Services
{
    public interface ITaskApiService
    {
        Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(Guid projectId);
        Task<TaskDto> GetTaskByIdAsync(Guid id);
        Task<TaskDto> CreateTaskAsync(CreateTaskDto dto);
        Task UpdateTaskAsync(Guid id, CreateTaskDto dto);
        Task DeleteTaskAsync(Guid id);
    }
}
