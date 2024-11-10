using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Metro.Models
{
    public class Task
    {
        public int Id { get; set; }
        public string Description { get; set; } = null!;
        public DateTime StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public int WorkDayId { get; set; }

        public Task() { }

        public Task(string description, DateTime startTime, int workDayId)
        {
            Description = description;
            StartTime = startTime;
            WorkDayId = workDayId;
        }

        public Task(string description, DateTime startTime, DateTime endTime, int workDayId)
        {
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            WorkDayId = workDayId;
        }

        public override string ToString() => JsonSerializer.Serialize<Task>(this);
    }
}
