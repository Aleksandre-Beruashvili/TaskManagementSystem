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
    public class ActivityLogService : IActivityLogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ActivityLogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task LogActivityAsync(ActivityLogDto dto)
        {
            var log = new ActivityLog
            {
                ActivityDate = dto.ActivityDate,
                ActivityType = dto.ActivityType,
                Description = dto.Description,
                ProjectId = dto.ProjectId,
                TaskId = dto.TaskId
            };
            await _unitOfWork.ActivityLogRepository.AddAsync(log);
            await _unitOfWork.CommitAsync();
        }

        public async Task<IEnumerable<ActivityLogDto>> GetActivityLogsAsync(DateTime? from = null, DateTime? to = null)
        {
            var logs = await _unitOfWork.ActivityLogRepository.GetAllAsync();
            if (from.HasValue)
                logs = logs.Where(l => l.ActivityDate >= from.Value);
            if (to.HasValue)
                logs = logs.Where(l => l.ActivityDate <= to.Value);
            return logs.Select(l => new ActivityLogDto
            {
                Id = l.Id,
                ActivityDate = l.ActivityDate,
                ActivityType = l.ActivityType,
                Description = l.Description,
                ProjectId = l.ProjectId,
                TaskId = l.TaskId
            });
        }
    }
}
