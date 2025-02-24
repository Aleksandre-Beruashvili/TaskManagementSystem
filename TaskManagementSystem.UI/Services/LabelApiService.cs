using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.UI.Services
{
    public class LabelApiService : ILabelApiService
    {
        private readonly HttpClient _httpClient;
        public LabelApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<LabelDto>> GetAllLabelsAsync()
        {
            return await _httpClient.GetFromJsonAsync<IEnumerable<LabelDto>>("api/labels");
        }

        public async Task<LabelDto> GetLabelByIdAsync(Guid id)
        {
            return await _httpClient.GetFromJsonAsync<LabelDto>($"api/labels/{id}");
        }

        public async Task<LabelDto> CreateLabelAsync(CreateLabelDto dto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/labels", dto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<LabelDto>();
        }

        public async Task UpdateLabelAsync(Guid id, CreateLabelDto dto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/labels/{id}", dto);
            response.EnsureSuccessStatusCode();
        }

        public async Task DeleteLabelAsync(Guid id)
        {
            var response = await _httpClient.DeleteAsync($"api/labels/{id}");
            response.EnsureSuccessStatusCode();
        }
    }
}
