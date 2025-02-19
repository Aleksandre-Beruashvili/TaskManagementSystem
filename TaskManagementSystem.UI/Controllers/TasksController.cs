using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.UI.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IProjectService _projectService;

        public TasksController(ITaskService taskService, IProjectService projectService)
        {
            _taskService = taskService;
            _projectService = projectService;
        }

        // GET: /Tasks/Index?projectId={projectId}
        [HttpGet]
        public async Task<IActionResult> Index(Guid projectId)
        {
            // Optionally, load project details to display on the page
            ViewBag.Project = await _projectService.GetProjectByIdAsync(projectId);
            var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
            return View(tasks);
        }

        // GET: /Tasks/Details/{id}
        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();
            return View(task);
        }

        // GET: /Tasks/Create?projectId={projectId}
        [HttpGet]
        public async Task<IActionResult> Create(Guid projectId)
        {
            // Load project details to display in the view (optional)
            ViewBag.Project = await _projectService.GetProjectByIdAsync(projectId);
            var dto = new CreateTaskDto
            {
                ProjectId = projectId,
                DueDate = DateTime.Now.AddDays(7)
            };
            return View(dto);
        }

        // POST: /Tasks/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            if (ModelState.IsValid)
            {
                var task = await _taskService.CreateTaskAsync(dto);
                return RedirectToAction("Details", new { id = task.Id });
            }
            return View(dto);
        }

        // GET: /Tasks/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();

            var dto = new CreateTaskDto
            {
                Title = task.Title,
                Description = task.Description,
                DueDate = task.DueDate,
                Priority = task.Priority,
                ProjectId = task.ProjectId,
                AssignedUserId = task.AssignedUserId
            };
            return View(dto);
        }

        // POST: /Tasks/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateTaskDto dto)
        {
            if (ModelState.IsValid)
            {
                await _taskService.UpdateTaskAsync(id, dto);
                return RedirectToAction("Details", new { id = id });
            }
            return View(dto);
        }

        // GET: /Tasks/Delete/{id}
        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();
            return View(task);
        }

        // POST: /Tasks/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _taskService.DeleteTaskAsync(id);
            // Optionally, redirect to the tasks list of the associated project.
            return RedirectToAction("Index");
        }
    }
}
