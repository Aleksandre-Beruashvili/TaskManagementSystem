using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Application.DTOs
{
    public class TeamDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<string> MemberIds { get; set; } = new List<string>();
    }
}
