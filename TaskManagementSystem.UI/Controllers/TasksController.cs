using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.UI.Services;

namespace TaskManagementSystem.UI.Controllers
{
    public class TasksController : Controller
    {
        private readonly ITaskApiService _taskApiService;
        private readonly IProjectApiService _projectApiService;

        public TasksController(ITaskApiService taskApiService, IProjectApiService projectApiService)
        {
            _taskApiService = taskApiService;
            _projectApiService = projectApiService;
        }

        public async Task<IActionResult> Index(Guid projectId)
        {
            ViewBag.Project = await _projectApiService.GetProjectByIdAsync(projectId);
            var tasks = await _taskApiService.GetTasksByProjectIdAsync(projectId);
            return View(tasks);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var task = await _taskApiService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();
            return View(task);
        }

        public IActionResult Create(Guid projectId)
        {
            var dto = new CreateTaskDto
            {
                ProjectId = projectId,
                DueDate = DateTime.Now.AddDays(7)
            };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTaskDto dto)
        {
            if (ModelState.IsValid)
            {
                var task = await _taskApiService.CreateTaskAsync(dto);
                return RedirectToAction(nameof(Details), new { id = task.Id });
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var task = await _taskApiService.GetTaskByIdAsync(id);
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateTaskDto dto)
        {
            if (ModelState.IsValid)
            {
                await _taskApiService.UpdateTaskAsync(id, dto);
                return RedirectToAction(nameof(Details), new { id = id });
            }
            return View(dto);
        }

        public async Task<IActionResult> Delete(Guid id)
        {
            var task = await _taskApiService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();
            return View(task);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _taskApiService.DeleteTaskAsync(id);
            return RedirectToAction("Index");
        }
    }
}
