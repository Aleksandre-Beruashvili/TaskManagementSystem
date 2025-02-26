using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskManagementSystem.UI.Services;

namespace TaskManagementSystem.UI.Controllers
{
    public class TeamController : Controller
    {
        private readonly ITeamApiService _teamApiService;
        public TeamController(ITeamApiService teamApiService)
        {
            _teamApiService = teamApiService;
        }

        public async Task<IActionResult> Index()
        {
            var teams = await _teamApiService.GetAllTeamsAsync();
            return View(teams);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var team = await _teamApiService.GetTeamByIdAsync(id);
            if (team == null) return NotFound();
            return View(team);
        }

        public IActionResult Create() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(TaskManagementSystem.Application.DTOs.TeamDto dto)
        {
            if (ModelState.IsValid)
            {
                var team = await _teamApiService.CreateTeamAsync(dto);
                return RedirectToAction(nameof(Details), new { id = team.Id });
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var team = await _teamApiService.GetTeamByIdAsync(id);
            if (team == null) return NotFound();
            return View(team);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, TaskManagementSystem.Application.DTOs.TeamDto dto)
        {
            if (ModelState.IsValid)
            {
                // Assuming your service supports updating the team.
                // You might need to implement a dedicated EditTeamAsync method.
                return RedirectToAction(nameof(Details), new { id });
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var team = await _teamApiService.GetTeamByIdAsync(id);
            if (team == null) return NotFound();
            return View(team);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            // Assuming your service supports deletion of teams.
            await _teamApiService.DeleteTeamAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
