using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.Application.Interfaces
{
    public interface ILabelService
    {
        Task<LabelDto> CreateLabelAsync(CreateLabelDto dto);
        Task<LabelDto> GetLabelByIdAsync(Guid id);
        Task<IEnumerable<LabelDto>> GetAllLabelsAsync();
        Task UpdateLabelAsync(Guid id, CreateLabelDto dto);
        Task DeleteLabelAsync(Guid id);
    }
}
