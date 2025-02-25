using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.UI.Services
{
    public class TaskApiService : ITaskApiService
    {
        private readonly HttpClient _httpClient;
        public TaskApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<TaskDto>> GetTasksByProjectIdAsync(Guid projectId)
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<TaskDto>>($"api/tasks/project/{projectId}");
        }

        public async Task<TaskDto> GetTaskByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<TaskDto>($"api/tasks/{id}");
        }

        public async Task<TaskDto> CreateTaskAsync(CreateTaskDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/tasks", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TaskDto>();
        }

        public async Task UpdateTaskAsync(Guid id, CreateTaskDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/tasks/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteTaskAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/tasks/{id}");
            response.EnsureSuccessStatusCode();
        }

        public async Task<IEnumerable<TaskDto>> GetAllTasksAsync()
        {
            // Calls GET /api/tasks/all
            return await _httpClient.GetFromJsonAsync<IEnumerable<TaskDto>>("api/tasks/all");
        }
    }
}
