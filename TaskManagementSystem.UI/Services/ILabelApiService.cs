using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Application.DTOs;

namespace TaskManagementSystem.UI.Services
{
    public interface ILabelApiService
    {
        Task<IEnumerable<LabelDto>> GetAllLabelsAsync();
        Task<LabelDto> GetLabelByIdAsync(Guid id);
        Task<LabelDto> CreateLabelAsync(CreateLabelDto dto);
        Task UpdateLabelAsync(Guid id, CreateLabelDto dto);
        Task DeleteLabelAsync(Guid id);
    }
}
