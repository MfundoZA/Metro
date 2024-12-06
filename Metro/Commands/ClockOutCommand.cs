using Metro.Data;
using Metro.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Metro.Commands
{
    public class ClockOutCommand : Command<ClockOutSettings>
    {
        private const string TIME_FORMAT = "HH:MM";

        public override int Execute([NotNull] CommandContext context, [NotNull] ClockOutSettings settings)
        {

            TimeOnly clockOutTime;
            DateTime tempClockOutTime;
            WorkDay? currentWorkDay;
            WorkDayQueries workDayQueries = new();

            List<WorkDay> workDays = workDayQueries.GetWorkDays();

            if (settings.Time != null)
            {
                if (DateTime.TryParse(settings.Time, out tempClockOutTime) == true)
                {
                    clockOutTime = TimeOnly.FromDateTime(tempClockOutTime);
                }
                else
                {
                    Console.Error.WriteLine("Error! Time format is incorrect. Please try again and ensure the format is " + TIME_FORMAT);
                    return -1;
                }
            }
            else
            {
                clockOutTime = TimeOnly.FromDateTime(DateTime.Now);
            }

           currentWorkDay = workDays.Where(x => x.WorkDate == DateOnly.FromDateTime(DateTime.Today.Date)).FirstOrDefault();

            if (workDays.Count == 0 || currentWorkDay == null || currentWorkDay.ClockInTime == TimeOnly.MinValue)
            {
                Console.Error.WriteLine("Error! Can not clock out without having clocked in first.");
                return -1;
            }


            if (settings.Force == true)
            {
                currentWorkDay.ClockOutTime = clockOutTime;
            }
            else
            {
                if (AnsiConsole.Confirm("Warning! You have already clocked out today. Do you want to override?"))
                {
                    workDayQueries.UpdateWorkDay(currentWorkDay);
                }
            }

            Console.WriteLine("[green underline]" + "Bye[/]!");
            return 0;
        }
    }
}
