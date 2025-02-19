using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface ITeamService
    {
        Task<TeamDto> CreateTeamAsync(TeamDto dto);
        Task<TeamDto> GetTeamByIdAsync(Guid id);
        Task<IEnumerable<TeamDto>> GetAllTeamsAsync();
        Task AddMemberAsync(Guid teamId, string userId);
        Task RemoveMemberAsync(Guid teamId, string userId);
    }
}
