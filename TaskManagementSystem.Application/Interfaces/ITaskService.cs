using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface ITaskService
    {
        Task<TaskDto> CreateTaskAsync(CreateTaskDto dto);
        Task<TaskDto> GetTaskByIdAsync(Guid id);
        Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(Guid projectId);
        Task UpdateTaskAsync(Guid id, CreateTaskDto dto);
        Task DeleteTaskAsync(Guid id);
        Task<IEnumerable<TaskDto>> SearchTasksAsync(string keyword);
        Task<IEnumerable<TaskDto>> GetAllTasksAsync();

    }
}
