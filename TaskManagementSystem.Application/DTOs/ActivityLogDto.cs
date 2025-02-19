using System;

namespace TaskManagementSystem.Application.DTOs
{
    public class ActivityLogDto
    {
        public Guid Id { get; set; }
        public DateTime ActivityDate { get; set; }
        public string ActivityType { get; set; }
        public string Description { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? TaskId { get; set; }
    }
}
