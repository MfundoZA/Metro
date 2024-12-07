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
                    AnsiConsole.Markup("[red underline]" + "Error! Time format is incorrect.[/] Please try again and ensure the format is \n" + TIME_FORMAT);
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
                AnsiConsole.Markup("[red underline]" + "Error:[/] Can not clock out without having clocked in first.");
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

            AnsiConsole.Markup("MMMMMMMM               MMMMMMMM                             tttt                                               \n");
            AnsiConsole.Markup("M:::::::M             M:::::::M                          ttt:::t                                               \n"); 
            AnsiConsole.Markup("M::::::::M           M::::::::M                          t:::::t                                               \n");
            AnsiConsole.Markup("M:::::::::M         M:::::::::M                          t:::::t                                               \n");
            AnsiConsole.Markup("M::::::::::M       M::::::::::M    eeeeeeeeeeee    ttttttt:::::ttttttt   rrrrr   rrrrrrrrr      ooooooooooo    \n");
            AnsiConsole.Markup("M:::::::::::M     M:::::::::::M  ee::::::::::::ee  t:::::::::::::::::t   r::::rrr:::::::::r   oo:::::::::::oo  \n");
            AnsiConsole.Markup("M:::::::M::::M   M::::M:::::::M e::::::eeeee:::::eet:::::::::::::::::t   r:::::::::::::::::r o:::::::::::::::o \n");
            AnsiConsole.Markup("M::::::M M::::M M::::M M::::::Me::::::e     e:::::etttttt:::::::tttttt   rr::::::rrrrr::::::ro:::::ooooo:::::o \n");
            AnsiConsole.Markup("M::::::M  M::::M::::M  M::::::Me:::::::eeeee::::::e      t:::::t          r:::::r     r:::::ro::::o     o::::o \n");
            AnsiConsole.Markup("M::::::M   M:::::::M   M::::::Me:::::::::::::::::e       t:::::t          r:::::r     rrrrrrro::::o     o::::o \n");
            AnsiConsole.Markup("M::::::M    M:::::M    M::::::Me::::::eeeeeeeeeee        t:::::t          r:::::r            o::::o     o::::o \n");
            AnsiConsole.Markup("M::::::M     MMMMM     M::::::Me:::::::e                 t:::::t    ttttttr:::::r            o::::o     o::::o \n");
            AnsiConsole.Markup("M::::::M               M::::::Me::::::::e                t::::::tttt:::::tr:::::r            o:::::ooooo:::::o \n");
            AnsiConsole.Markup("M::::::M               M::::::M e::::::::eeeeeeee        tt::::::::::::::tr:::::r            o:::::::::::::::o \n");
            AnsiConsole.Markup("M::::::M               M::::::M  ee:::::::::::::e          tt:::::::::::ttr:::::r             oo:::::::::::oo \n");
            AnsiConsole.Markup("MMMMMMMM               MMMMMMMM    eeeeeeeeeeeeee            ttttttttttt  rrrrrrr               ooooooooooo \n");

            return 0;
        }
    }
}
