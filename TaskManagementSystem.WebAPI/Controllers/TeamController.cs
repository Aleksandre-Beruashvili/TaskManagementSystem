using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromBody] TeamDto dto)
        {
            var team = await _teamService.CreateTeamAsync(dto);
            return CreatedAtAction(nameof(GetTeamById), new { id = team.Id }, team);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTeamById(Guid id)
        {
            var team = await _teamService.GetTeamByIdAsync(id);
            if (team == null)
                return NotFound();
            return Ok(team);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTeams()
        {
            var teams = await _teamService.GetAllTeamsAsync();
            return Ok(teams);
        }

        [HttpPost("{id}/addmember")]
        public async Task<IActionResult> AddMember(Guid id, [FromBody] string userId)
        {
            await _teamService.AddMemberAsync(id, userId);
            return Ok("Member added.");
        }

        [HttpPost("{id}/removemember")]
        public async Task<IActionResult> RemoveMember(Guid id, [FromBody] string userId)
        {
            await _teamService.RemoveMemberAsync(id, userId);
            return Ok("Member removed.");
        }
    }
}
