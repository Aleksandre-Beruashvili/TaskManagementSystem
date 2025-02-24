namespace TaskManagementSystem.UI.Services
{
    public interface ITokenService
    {
        string GetToken();
        void SetToken(string token);
    }
}
