using System.Net.Http;
using System.Threading.Tasks;

namespace TaskManagementSystem.UI.Services
{
    public class ProfileApiService : IProfileApiService
    {
        private readonly HttpClient _httpClient;

        public ProfileApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task DeleteAccountAsync()
        {
            // Call the WebAPI endpoint to delete the current user's account.
            var response = await _httpClient.DeleteAsync("api/profile/deleteaccount");
            response.EnsureSuccessStatusCode();
        }
    }
}
