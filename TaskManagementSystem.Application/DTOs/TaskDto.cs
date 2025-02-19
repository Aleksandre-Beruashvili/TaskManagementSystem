using System;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.DTOs
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskState State { get; set; }  // Renamed enum
        public PriorityLevel Priority { get; set; }
        public Guid ProjectId { get; set; }
        public string AssignedUserId { get; set; } // NEW
    }
}
