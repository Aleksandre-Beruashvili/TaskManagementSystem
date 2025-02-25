using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;
using TaskManagementSystem.UI.Services;

namespace TaskManagementSystem.UI.Controllers
{
    public class LabelsController : Controller
    {
        private readonly ILabelApiService _labelApiService;

        public LabelsController(ILabelApiService labelApiService)
        {
            _labelApiService = labelApiService;
        }

        public async Task<IActionResult> Index()
        {
            var labels = await _labelApiService.GetAllLabelsAsync();
            return View(labels);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var label = await _labelApiService.GetLabelByIdAsync(id);
            if (label == null)
                return NotFound();
            return View(label);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateLabelDto dto)
        {
            if (ModelState.IsValid)
            {
                var label = await _labelApiService.CreateLabelAsync(dto);
                return RedirectToAction(nameof(Details), new { id = label.Id });
            }
            return View(dto);
        }

        public async Task<IActionResult> Edit(Guid id)
        {
            var label = await _labelApiService.GetLabelByIdAsync(id);
            if (label == null)
                return NotFound();
            var dto = new CreateLabelDto { Name = label.Name };
            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, CreateLabelDto dto)
        {
            if (ModelState.IsValid)
            {
                await _labelApiService.UpdateLabelAsync(id, dto);
                return RedirectToAction(nameof(Details), new { id = id });
            }
            return View(dto);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(Guid id)
        {
            var label = await _labelApiService.GetLabelByIdAsync(id);
            if (label == null)
                return NotFound();
            return View(label);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _labelApiService.DeleteLabelAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
