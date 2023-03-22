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
        public string Description { get; set; }
        public List<Session> Sessions { get; set; }
        public bool IsComplete { get; set; }

        public Task() { }

        public Task(string description, List<Session> sessions, bool isComplete)
        {
            Description = description;
            Sessions = sessions;
            IsComplete = isComplete;
        }

        public override string ToString() => JsonSerializer.Serialize<Task>(this);
    }
}
