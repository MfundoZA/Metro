using Metro.Data;
using Metro.Models;
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;

namespace Metro.Commands
{
    public class ClockInCommand : Command<ClockInSettings>
    {
        private const string TIME_FORMAT = "HH:MM";
        
        public override int Execute([NotNull] CommandContext context, [NotNull] ClockInSettings settings)
        {
            DateTime tempClockInTime;
            TimeOnly clockInTime;
            WorkDay currentWorkday;
            WorkDayQueries workDayQueries = new();

            if (settings.Time == null)
            {
                tempClockInTime = DateTime.Now;
                clockInTime = TimeOnly.FromDateTime(tempClockInTime);
            }
            else
            {
                if (DateTime.TryParse(settings.Time, out tempClockInTime) == false)
                {
                    AnsiConsole.Markup("[underline red]" + "Error! Time format is incorrect.[/] Please try again and ensure the format is as follows: " + TIME_FORMAT);
                    return -1;
                }

                clockInTime = TimeOnly.FromDateTime(tempClockInTime);
            }


            bool workdaysExist = workDayQueries.GetWorkDaysCount() > 0;
            WorkDay? previousWorkDay;

            previousWorkDay = workDayQueries.GetWorkDays().LastOrDefault();

            if (workdaysExist == true)
            {

                currentWorkday = new WorkDay(previousWorkDay.Id + 1, DateOnly.FromDateTime(DateTime.Now), clockInTime, null);
            }
            else
            {
                currentWorkday = new WorkDay(1, DateOnly.FromDateTime(DateTime.Now), clockInTime, null);
            }

            if (workdaysExist == true && previousWorkDay?.WorkDate == currentWorkday.WorkDate)
            {
                if (settings.Force == true || AnsiConsole.Confirm("Warning! You have already clocked in today. Do you want to override?"))
                {
                    currentWorkday.Id = previousWorkDay.Id;
                    workDayQueries.UpdateWorkDay(currentWorkday);
                }
            }
            else
            {
                workDayQueries.CreateNewWorkDay(currentWorkday);
            }

            AnsiConsole.Markup("Successfully clocked in @ [underline]" + currentWorkday.ClockInTime.ToShortTimeString() + "[/]");

            return 0;
        }
    }
}
