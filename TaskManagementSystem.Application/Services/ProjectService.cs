using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Interfaces;

namespace TaskManagementSystem.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto)
        {
            var project = new Project
            {
                Name = dto.Name,
                Description = dto.Description,
                DueDate = dto.DueDate
            };
            await _unitOfWork.ProjectRepository.AddAsync(project);
            await _unitOfWork.CommitAsync();
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                DueDate = project.DueDate
            };
        }

        public async Task DeleteProjectAsync(Guid id)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (project == null)
                throw new Exception("Project not found");
            _unitOfWork.ProjectRepository.Remove(project);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            var projects = await _unitOfWork.ProjectRepository.GetAllAsync();
            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DueDate = p.DueDate
            });
        }

        public async Task<ProjectDto> GetProjectByIdAsync(Guid id)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (project == null)
                throw new Exception("Project not found");
            return new ProjectDto
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                DueDate = project.DueDate
            };
        }

        public async Task UpdateProjectAsync(Guid id, CreateProjectDto dto)
        {
            var project = await _unitOfWork.ProjectRepository.GetByIdAsync(id);
            if (project == null)
                throw new Exception("Project not found");
            project.Name = dto.Name;
            project.Description = dto.Description;
            project.DueDate = dto.DueDate;
            _unitOfWork.ProjectRepository.Update(project);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ProjectDto>> SearchProjectsAsync(string keyword)
        {
            var projects = await _unitOfWork.ProjectRepository.GetAllAsync();
            var filtered = projects.Where(p => p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)
                                              || p.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            return filtered.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                DueDate = p.DueDate
            });
        }
    }
}
