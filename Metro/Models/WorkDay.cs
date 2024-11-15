﻿using System;
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
        public DateTime ClockInTime { get; set; }
        public DateTime? ClockOutTime { get; set;}

        public WorkDay()
        {

        }

        public WorkDay(int id, DateTime clockInTime)
        {
            Id = id;
            ClockInTime = clockInTime;
        }

        public WorkDay(DateTime clockInTime, DateTime clockOutTime)
        {
            ClockInTime = clockInTime;
            ClockOutTime = clockOutTime;
        }

        public override string ToString() => JsonSerializer.Serialize<WorkDay>(this);
    }
}
