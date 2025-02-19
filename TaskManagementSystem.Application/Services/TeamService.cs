using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;
using TaskManagementSystem.Domain.Entities;
using TaskManagementSystem.Domain.Interfaces;

namespace TaskManagementSystem.Application.Services
{
    public class TeamService : ITeamService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TeamService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TeamDto> CreateTeamAsync(TeamDto dto)
        {
            var team = new Team
            {
                Name = dto.Name,
                MemberIds = dto.MemberIds
            };
            await _unitOfWork.TeamRepository.AddAsync(team);
            await _unitOfWork.CommitAsync();
            return new TeamDto { Id = team.Id, Name = team.Name, MemberIds = team.MemberIds.ToList() };
        }

        public async Task AddMemberAsync(Guid teamId, string userId)
        {
            var team = await _unitOfWork.TeamRepository.GetByIdAsync(teamId);
            if (team == null)
                throw new Exception("Team not found");
            team.MemberIds.Add(userId);
            _unitOfWork.TeamRepository.Update(team);
            await _unitOfWork.CommitAsync();
        }

        public async Task RemoveMemberAsync(Guid teamId, string userId)
        {
            var team = await _unitOfWork.TeamRepository.GetByIdAsync(teamId);
            if (team == null)
                throw new Exception("Team not found");
            team.MemberIds.Remove(userId);
            _unitOfWork.TeamRepository.Update(team);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<TeamDto>> GetAllTeamsAsync()
        {
            var teams = await _unitOfWork.TeamRepository.GetAllAsync();
            return teams.Select(t => new TeamDto
            {
                Id = t.Id,
                Name = t.Name,
                MemberIds = t.MemberIds.ToList()
            });
        }

        public async Task<TeamDto> GetTeamByIdAsync(Guid id)
        {
            var team = await _unitOfWork.TeamRepository.GetByIdAsync(id);
            if (team == null)
                throw new Exception("Team not found");
            return new TeamDto
            {
                Id = team.Id,
                Name = team.Name,
                MemberIds = team.MemberIds.ToList()
            };
        }
    }
}
