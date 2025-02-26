using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.UI.Services
{
    public interface ITeamApiService
    {
        Task<IEnumerable<TeamDto>> GetAllTeamsAsync();
        Task<TeamDto> GetTeamByIdAsync(Guid id);
        Task<TeamDto> CreateTeamAsync(TeamDto dto);
        Task UpdateTeamAsync(Guid id, TeamDto dto);
        Task DeleteTeamAsync(Guid id);
        Task AddMemberAsync(Guid teamId, string userId);
        Task RemoveMemberAsync(Guid teamId, string userId);
    }
}
