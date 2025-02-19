using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Domain.Entities
{
    public class Project
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    }
}
