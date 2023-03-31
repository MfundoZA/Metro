using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Metro.Models
{
    public class Session
    {
        public int Id { get; set; }
        public TimeOnly StartTime { get; set; }
        public TimeOnly? EndTime { get; set; }

        public Session() { }

        public Session(TimeOnly startTime, TimeOnly endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public override string ToString() => JsonSerializer.Serialize<Session>(this);
    }
}
