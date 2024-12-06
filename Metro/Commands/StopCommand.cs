using Metro.Models;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Task = Metro.Models.Task;
using Spectre.Console;
using System.Text.Json;
using Metro.Data;

namespace Metro.Commands
{
    public class StopCommand : Command<StopSettings>
    {

        public override int Execute([NotNull] CommandContext context, [NotNull] StopSettings settings)
        {
            WorkDayQueries workDayQueries = new();
            TaskQueries taskQueries = new();

            /* Technical Debt */
            // Pull all tasks and find tasks that have null end times
            List<WorkDay>? workDays = workDayQueries.GetWorkDays();
            WorkDay? currentWorkDay = workDays.Where(x => x.WorkDate == DateOnly.FromDateTime(DateTime.Now)).FirstOrDefault();

            List<Task>? tasks = null;

            if (currentWorkDay != null)
            {
                tasks = taskQueries.GetTasks().Where(x => x.WorkDayId == currentWorkDay.Id).ToList();
            }
            List<Task>? currentTasks = new List<Task>();
            List<string>? currentTasksDescriptions = new List<string>();
            List<string>? tasksToStop = new List<string>();

            if (workDays == null || workDays.Last<WorkDay>().WorkDate == DateOnly.FromDateTime(DateTime.Today.Date.AddDays(-1)))
            {
                AnsiConsole.Markup("[red underline]" + "Error! You are currently not clocked in.[/] Please clock in and try again.");
                return -1;
            }

            if (tasks == null || tasks.Count == 0)
            {
                AnsiConsole.Markup("[red underline]" + "Error! No tasks found.[/] Please make sure you have tasks that need to be stopped and try again.");
                return -1;
            }

            var areTasksCurrentlyTracked = tasks.Where(x => x.EndTime == TimeOnly.MinValue).ToList().Count > 0;

            if (areTasksCurrentlyTracked == false)
            {
                AnsiConsole.Markup("[red underline]" + "Error! No tasks found.[/] Please make sure you have tasks that need to be stopped and try again.");
                return -1;
            }

            foreach (var task in tasks)
            {
                if (task.EndTime == TimeOnly.MinValue)
                {
                    currentTasks.Add(task);
                    currentTasksDescriptions.Add(task.Description);
                }
            }

            if (currentTasks != null)
            {
                tasksToStop = AnsiConsole.Prompt(
                    new MultiSelectionPrompt<string>()
                    .Title("What tasks would you like to stop?")
                    .MoreChoicesText("[grey](Move up and down to reveal more tasks)[/]")
                    .InstructionsText("[grey](Press [blue]<space>[/] to choose one or more tasks, " +
                    "[green]<enter>[/] to accept)[/]")
                    .AddChoices(currentTasksDescriptions));

                var timeFinished = DateTime.Now;

                foreach (string taskDescription in tasksToStop)
                {
                    Task taskSelected = tasks.Where(x => x.Description == taskDescription).First();
                    taskSelected.EndTime = TimeOnly.FromDateTime(timeFinished);

                    taskQueries.UpdateTask(taskSelected);
                }
            }

            Console.WriteLine("Task(s) stopped successfully.");
            return 0;
        }
    }
}
