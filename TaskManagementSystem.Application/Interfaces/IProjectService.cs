using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface IProjectService
    {
        Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto);
        Task<ProjectDto> GetProjectByIdAsync(Guid id);
        Task<IEnumerable<ProjectDto>> GetAllProjectsAsync();
        Task UpdateProjectAsync(Guid id, CreateProjectDto dto);
        Task DeleteProjectAsync(Guid id);
        Task<IEnumerable<ProjectDto>> SearchProjectsAsync(string keyword);
    }
}
