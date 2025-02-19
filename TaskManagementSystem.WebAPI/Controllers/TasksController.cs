using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.Application.Interfaces;

namespace TaskManagementSystem.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto dto)
        {
            var result = await _taskService.CreateTaskAsync(dto);
            return CreatedAtAction(nameof(GetTaskById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var task = await _taskService.GetTaskByIdAsync(id);
            if (task == null)
                return NotFound();
            return Ok(task);
        }

        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTasksByProjectId(Guid projectId)
        {
            var tasks = await _taskService.GetTasksByProjectIdAsync(projectId);
            return Ok(tasks);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody] CreateTaskDto dto)
        {
            await _taskService.UpdateTaskAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            await _taskService.DeleteTaskAsync(id);
            return NoContent();
        }

        // NEW: Search endpoint for tasks.
        [HttpGet("search")]
        public async Task<IActionResult> SearchTasks([FromQuery] string keyword)
        {
            var tasks = await _taskService.SearchTasksAsync(keyword);
            return Ok(tasks);
        }
    }
}
