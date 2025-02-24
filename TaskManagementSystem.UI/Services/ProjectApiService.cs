using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.UI.Services
{
    public class ProjectApiService : IProjectApiService
    {
        private readonly HttpClient _httpClient;
        public ProjectApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllProjectsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<ProjectDto>>("api/projects");
        }

        public async Task<ProjectDto> GetProjectByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<ProjectDto>($"api/projects/{id}");
        }

        public async Task<ProjectDto> CreateProjectAsync(CreateProjectDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/projects", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ProjectDto>();
        }

        public async Task UpdateProjectAsync(Guid id, CreateProjectDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/projects/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteProjectAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/projects/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
