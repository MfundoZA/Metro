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

            DateTime clockOutTime;
            WorkDay? currentWorkDay;

            List<WorkDay> workDays = workDayQueries.GetWorkDays();

            if (settings.Time == null)
            {
                clockOutTime = DateTime.Now;
            }
            else
            {
                if (DateTime.TryParse(settings.Time, out clockOutTime) == false)
                {
                    Console.Error.WriteLine("Error! Time format is incorrect. Please try again and ensure the format is " + TIME_FORMAT);
                    return -1;
                }
            }

            if (workDays != null)
            {
                currentWorkDay = workDays.Where(x => x.ClockInTime.Date == DateTime.Today.Date).FirstOrDefault();

                if (currentWorkDay != null)
                {
                    var currentWorkDayIndex = workDays.ToList().IndexOf(currentWorkDay);

                    if ((currentWorkDay.ClockOutTime == null) || (currentWorkDay.ClockOutTime.GetValueOrDefault().Date == DateTime.Today.Date && settings.Force == true))
                    {
                        currentWorkDay.ClockOutTime = clockOutTime;
                    }
                    else if (currentWorkDay.ClockOutTime != null && settings.Force == false)
                    {
                        if (AnsiConsole.Confirm("Warning! You have already clocked out today. Do you want to override?"))
                        {
                            workDays.RemoveAt(currentWorkDayIndex);
                            workDays.Insert(currentWorkDayIndex, currentWorkDay);
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Error! Can not clock out without having clocked in first.");
                        return -1;
                    }
                }
            }
            else
            {
                Console.Error.WriteLine("Error! Can not clock out without having clocked in.");
                return -1;
            }

            Console.WriteLine("[green underline]" + "Bye[/]!");
            return 0;
        }
    }
}
