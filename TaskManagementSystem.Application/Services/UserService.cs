using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.Application.Services
{
    public class UserService : IUserService
    {
        public async Task RegisterUserAsync(RegisterUserDto dto)
        {
            // Implement additional registration logic if required.
            await Task.CompletedTask;
        }
    }
}
