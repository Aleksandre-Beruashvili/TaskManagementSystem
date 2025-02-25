using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.UI.Services;

namespace TaskManagementSystem.UI.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectApiService _projectApiService;

        public ProjectsController(IProjectApiService projectApiService)
        {
            _projectApiService = projectApiService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectApiService.GetAllProjectsAsync();
            return View(projects);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var project = await _projectApiService.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();
            return View(project);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProjectDto dto)
        {
            if (ModelState.IsValid)
            {
                var project = await _projectApiService.CreateProjectAsync(dto);
                return RedirectToAction(nameof(Details), new { id = project.Id });
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var project = await _projectApiService.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();
            var dto = new CreateProjectDto
            {
                Name = project.Name,
                Description = project.Description,
                DueDate = project.DueDate
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateProjectDto dto)
        {
            if (ModelState.IsValid)
            {
                await _projectApiService.UpdateProjectAsync(id, dto);
                return RedirectToAction(nameof(Details), new { id = id });
            }
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var project = await _projectApiService.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _projectApiService.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
