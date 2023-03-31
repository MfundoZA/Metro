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
        public TimeOnly StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }
        public WorkDay WorkDay { get; set; } = null!;

        public Task() { }

        public Task(string description, TimeOnly startTime, WorkDay workDay)
        {
            Description = description;
            StartTime = startTime;
            WorkDay = workDay;
        }

        public Task(string description, TimeOnly startTime, TimeOnly endTime, WorkDay workDay)
        {
            Description = description;
            StartTime = startTime;
            EndTime = endTime;
            WorkDay = workDay;
        }

        public override string ToString() => JsonSerializer.Serialize<Task>(this);
    }
}
