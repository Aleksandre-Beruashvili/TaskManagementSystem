using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface IUserService
    {
        Task RegisterUserAsync(RegisterUserDto dto);
    }
}
