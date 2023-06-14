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
        public WorkDay WorkDay { get; set; } = null!;

        public Task() { }

        public Task(string description, DateTime startTime, WorkDay workDay)
        {
            Description = description;
            StartTime = startTime;
            WorkDay = workDay;
        }

        public Task(string description, DateTime startTime, DateTime endTime, WorkDay workDay)
        {
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            WorkDay = workDay;
        }

        public override string ToString() => JsonSerializer.Serialize<Task>(this);
    }
}
