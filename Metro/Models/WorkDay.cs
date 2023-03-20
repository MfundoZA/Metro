using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Models
{
    public class WorkDay
    {
        public DateTime ClockInTime { get; set; }
        public DateTime ClockOutTime { get; set;}

        public WorkDay()
        {

        }

        public WorkDay(DateTime clockInTime, DateTime clockOutTime)
        {
            ClockInTime = clockInTime;
            ClockOutTime = clockOutTime;
        }
    }
}
