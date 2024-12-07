using Metro.Data;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Commands
{
    public class ReportCommand : Command<ReportSettings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] ReportSettings settings)
        {
            TaskQueries taskQueries = new();
            WorkDayQueries workDayQueries = new();
            var todayWorkDay = workDayQueries.GetWorkDays().Where(x => x.WorkDate == DateOnly.FromDateTime(DateTime.Now));
            var tasks = taskQueries.GetTasks();

            var query = tasks.Join(todayWorkDay,
                        task => task.WorkDayId, workDay => workDay.Id,
                        (task, workDay) => new { Id = $"{task.Id}", Description = $"{task.Description}", StartTime = $"{task.StartTime}", EndTime = $"{task.EndTime}" });

            foreach (var task in query)
            {
                AnsiConsole.Markup($"Id: {task.Id} \n");
                AnsiConsole.Markup($"Description: {task.Description} \n");
                AnsiConsole.Markup($"Start Time: {task.StartTime} \n");
                AnsiConsole.Markup($"End Time: {task.EndTime} \n\n");
            }

            return 0;
        }
    }
}
