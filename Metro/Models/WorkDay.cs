using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Metro.Models
{
    public class WorkDay
    {
        public int Id { get; set; }
        public DateOnly WorkDate { get; set; }
        public TimeOnly ClockInTime { get; set; }
        public TimeOnly? ClockOutTime { get; set;}

        public WorkDay()
        {

        }

        public WorkDay(DateOnly workDate, TimeOnly clockInTime)
        {
            WorkDate = workDate;
            ClockInTime = clockInTime;
        }

        public WorkDay(int id, DateOnly workDate, TimeOnly clockInTime, TimeOnly? clockOutTime)
        {
            Id = id;
            WorkDate = workDate;
            ClockInTime = clockInTime;
            ClockOutTime = clockOutTime;
        }
    }
}
