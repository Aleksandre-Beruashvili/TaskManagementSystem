using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementSystem.Domain.Enums;

namespace TaskManagementSystem.Domain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public TaskState State { get; set; } = TaskState.NotStarted; // Renamed enum
        public PriorityLevel Priority { get; set; } = PriorityLevel.Medium;
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        // NEW: Assigned user ID (from Identity)
        public string AssignedUserId { get; set; }
        public ICollection<Label> Labels { get; set; } = new List<Label>();
    }
}
