using System;
using TaskManagementSystem.Domain.Entities;

namespace TaskManagementSystem.Application.DTOs
{
    public class ProjectDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }

    }
}
