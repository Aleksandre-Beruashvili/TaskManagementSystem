using System;
using System.Collections.Generic;

namespace TaskManagementSystem.Domain.Entities
{
    public class Team
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public ICollection<string> MemberIds { get; set; } = new List<string>();
    }
}
