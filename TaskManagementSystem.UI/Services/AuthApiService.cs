using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.UI.Services
{
    public class AuthApiService : IAuthApiService
    {
        private readonly HttpClient _httpClient;
        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(LoginUserDto loginDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);
            if (!response.IsSuccessStatusCode)
            {
                // Read error message from API.
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new System.Exception(errorMessage);
            }
            var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();
            return result.Token;
        }

        public async Task<string> RegisterAsync(RegisterUserDto registerDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }
}
