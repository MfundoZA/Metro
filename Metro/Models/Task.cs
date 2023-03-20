using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Models
{
    public class Task
    {
        public string Description { get; set; }
        public List<Tuple<DateTime, DateTime>> Sessions { get; set; }
        public bool IsComplete { get; set; }

        public Task() { }

        public Task(string description, List<Tuple<DateTime, DateTime>> sessions, bool isComplete)
        {
            Description = description;
            Sessions = sessions;
            IsComplete = isComplete;
        }
    }
}
