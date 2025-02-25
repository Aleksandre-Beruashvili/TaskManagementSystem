namespace TaskManagementSystem.UI.Services
{
    public class TokenService : ITokenService
    {
        private string _token;
        public string GetToken() => _token;
        public void SetToken(string token) => _token = token;
    }
}
