using System;

namespace TaskManagementSystem.Domain.Entities
{
    public class ActivityLog
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime ActivityDate { get; set; } = DateTime.UtcNow;
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? TaskId { get; set; }
    }
}
