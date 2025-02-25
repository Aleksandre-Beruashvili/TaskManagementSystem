using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Enums;
using TaskManagementSystem.Domain.Interfaces;

namespace TaskManagementSystem.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            var tasks = await _unitOfWork.TaskRepository.GetAllAsync();
            return tasks.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                State = t.State,
                ProjectId = t.ProjectId,
                AssignedUserId = t.AssignedUserId
            });
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto)
        {
            // Validate that the parent project exists
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(dto.ProjectId);
            if (project == null)
                throw new Exception("Project not found");

            var taskItem = new TaskItem
            {
                Title = dto.Title,
                Description = dto.Description,
                DueDate = dto.DueDate,
                Priority = dto.Priority,
                ProjectId = dto.ProjectId,
                State = TaskState.NotStarted, // Using renamed enum
                AssignedUserId = dto.AssignedUserId
            };

            await _unitOfWork.TaskRepository.AddAsync(taskItem);
            await _unitOfWork.CommitAsync();

            return new TaskDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                DueDate = taskItem.DueDate,
                Priority = taskItem.Priority,
                State = taskItem.State,
                ProjectId = taskItem.ProjectId,
                AssignedUserId = taskItem.AssignedUserId
            };
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            var taskItem = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (taskItem == null)
                throw new Exception("Task not found");
            _unitOfWork.TaskRepository.Remove(taskItem);
            await _unitOfWork.CommitAsync();
        }

        public async Task<TaskDto> GetTaskByIdAsync(Guid id)
        {
            var taskItem = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (taskItem == null)
                throw new Exception("Task not found");
            return new TaskDto
            {
                Id = taskItem.Id,
                Title = taskItem.Title,
                Description = taskItem.Description,
                DueDate = taskItem.DueDate,
                Priority = taskItem.Priority,
                State = taskItem.State,
                ProjectId = taskItem.ProjectId,
                AssignedUserId = taskItem.AssignedUserId
            };
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(Guid projectId)
        {
            var tasks = await _unitOfWork.TaskRepository.GetAllAsync();
            var filtered = tasks.Where(t => t.ProjectId == projectId);
            return filtered.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                State = t.State,
                ProjectId = t.ProjectId,
                AssignedUserId = t.AssignedUserId
            });
        }

        public async Task UpdateTaskAsync(Guid id, CreateTaskDto dto)
        {
            var taskItem = await _unitOfWork.TaskRepository.GetByIdAsync(id);
            if (taskItem == null)
                throw new Exception("Task not found");
            taskItem.Title = dto.Title;
            taskItem.Description = dto.Description;
            taskItem.DueDate = dto.DueDate;
            taskItem.Priority = dto.Priority;
            taskItem.ProjectId = dto.ProjectId;
            taskItem.AssignedUserId = dto.AssignedUserId;
            _unitOfWork.TaskRepository.Update(taskItem);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<TaskDto>> SearchTasksAsync(string keyword)
        {
            var tasks = await _unitOfWork.TaskRepository.GetAllAsync();
            var filtered = tasks.Where(t =>
                t.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                t.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            return filtered.Select(t => new TaskDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                DueDate = t.DueDate,
                Priority = t.Priority,
                State = t.State,
                ProjectId = t.ProjectId,
                AssignedUserId = t.AssignedUserId
            });

        }
    }
}
