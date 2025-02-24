namespace TaskManagementSystem.UI.Services
{
    public class TokenService : ITokenService
    {
        // In a real project, use secure storage (like session or cookies)
        private string _token;
        public string GetToken() => _token;
        public void SetToken(string token) => _token = token;
    }
}
