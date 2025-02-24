using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.UI.Services
{
    public interface IAuthApiService
    {
        Task<string> LoginAsync(LoginUserDto loginDto);
        Task<string> RegisterAsync(RegisterUserDto registerDto);
    }
}
