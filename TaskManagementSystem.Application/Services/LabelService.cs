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
    public class LabelService : ILabelService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LabelService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<LabelDto> CreateLabelAsync(CreateLabelDto dto)
        {
            var label = new Label { Name = dto.Name };
            await _unitOfWork.LabelRepository.AddAsync(label);
            await _unitOfWork.CommitAsync();
            return new LabelDto { Id = label.Id, Name = label.Name };
        }

        public async Task DeleteLabelAsync(Guid id)
        {
            var label = await _unitOfWork.LabelRepository.GetByIdAsync(id);
            if (label == null)
                throw new Exception("Label not found");
            _unitOfWork.LabelRepository.Remove(label);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<LabelDto>> GetAllLabelsAsync()
        {
            var labels = await _unitOfWork.LabelRepository.GetAllAsync();
            return labels.Select(l => new LabelDto { Id = l.Id, Name = l.Name });
        }

        public async Task<LabelDto> GetLabelByIdAsync(Guid id)
        {
            var label = await _unitOfWork.LabelRepository.GetByIdAsync(id);
            if (label == null)
                throw new Exception("Label not found");
            return new LabelDto { Id = label.Id, Name = label.Name };
        }

        public async Task UpdateLabelAsync(Guid id, CreateLabelDto dto)
        {
            var label = await _unitOfWork.LabelRepository.GetByIdAsync(id);
            if (label == null)
                throw new Exception("Label not found");
            label.Name = dto.Name;
            _unitOfWork.LabelRepository.Update(label);
            await _unitOfWork.CommitAsync();
        }
    }
}
