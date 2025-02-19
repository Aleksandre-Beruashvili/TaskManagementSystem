using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.UI.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectsController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IActionResult> Index()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            return View(projects);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
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
                var project = await _projectService.CreateProjectAsync(dto);
                return RedirectToAction(nameof(Details), new { id = project.Id });
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
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
                await _projectService.UpdateProjectAsync(id, dto);
                return RedirectToAction(nameof(Details), new { id = id });
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            if (project == null)
                return NotFound();
            return View(project);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _projectService.DeleteProjectAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
