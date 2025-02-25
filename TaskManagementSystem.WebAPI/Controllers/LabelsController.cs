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
    public class LabelsController : ControllerBase
    {
        private readonly ILabelService _labelService;
        public LabelsController(ILabelService labelService)
        {
            _labelService = labelService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateLabel([FromBody] CreateLabelDto dto)
        {
            var result = await _labelService.CreateLabelAsync(dto);
            return CreatedAtAction(nameof(GetLabelById), new { id = result.Id }, result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetLabelById(Guid id)
        {
            var label = await _labelService.GetLabelByIdAsync(id);
            if (label == null)
                return NotFound();
            return Ok(label);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllLabels()
        {
            var labels = await _labelService.GetAllLabelsAsync();
            return Ok(labels);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateLabel(Guid id, [FromBody] CreateLabelDto dto)
        {
            await _labelService.UpdateLabelAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLabel(Guid id)
        {
            await _labelService.DeleteLabelAsync(id);
            return NoContent();
        }
    }
}
