using System;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Application.DTOs
{
    public class CreateTaskDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public PriorityLevel Priority { get; set; }
        public Guid ProjectId { get; set; }
        public string AssignedUserId { get; set; }  // Optional: to assign a task
    }
}
