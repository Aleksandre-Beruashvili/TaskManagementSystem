using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.UI.Services
{
    public interface IProjectApiService
    {
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task<ProjectDto> GetProjectByIdAsync(Guid id);
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto);
        Task UpdateProjectAsync(Guid id, CreateProjectDto dto);
        Task DeleteProjectAsync(Guid id);
    }
}
