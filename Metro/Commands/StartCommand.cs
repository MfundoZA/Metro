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
using Task = Metro.Models.Task;

namespace Metro.Commands
{
    public class StartCommand : Command<StartSettings>
    {
        private const string TIME_FORMAT = "HH:MM";

        public override int Execute([NotNull] CommandContext context, [NotNull] StartSettings settings)
        {
            string taskDescription = settings.Description;
            DateTime startTime;
            TimeOnly? endTime;
            Task currentTask;
            WorkDayQueries workDayQueries = new();
            TaskQueries taskQueries = new();

            var currentWorkDay = workDayQueries.GetWorkDays().Where(x => x.WorkDate == DateOnly.FromDateTime(DateTime.Today)).FirstOrDefault();
            List<Task>? currentTasks;

            /* Technical Debt */
            if (currentWorkDay != null)
            {
                currentTasks = taskQueries.GetTasks()?.Where(x => x.WorkDayId == currentWorkDay.Id).ToList();

                if (currentTasks == null)
                {
                    currentTasks = new List<Task>();
                }

                var lastTask = currentTasks.Count == 0 ? null : currentTasks?.Last();

                if (settings.StartTime != null)
                {
                    if (DateTime.TryParse(settings.StartTime, out startTime) == false)
                    {
                        AnsiConsole.Markup("[underline red]" + "Error! Start time format is incorrect.[/] Please try again and ensure the format is as follows: " + TIME_FORMAT);
                        return -1;
                    }
                }
                else
                {
                    startTime = DateTime.Now;
                }

                if (settings.EndTime != null)
                {
                    DateTime tempEndTime;

                    if (DateTime.TryParse(settings.EndTime, out tempEndTime) == false)
                    {
                        AnsiConsole.Markup("[underline red]" + "Error! End time format is incorrect.[/] Please try again and ensure the format is as follows: " + TIME_FORMAT);
                        return -1;
                    }

                    // If parsing successful assign tempEndTime to endTime
                    endTime = TimeOnly.FromDateTime(tempEndTime);
                }
                else
                {
                    endTime = null;
                }

                if (lastTask != null)
                {
                    currentTask = new Task(lastTask.Id + 1, taskDescription, TimeOnly.FromDateTime(startTime), endTime, currentWorkDay.Id);
                }
                else
                {
                    currentTask = new Task(1, taskDescription, TimeOnly.FromDateTime(startTime), endTime, currentWorkDay.Id);
                }

                currentTasks.Add(currentTask);
            }
            else
            {
                AnsiConsole.Markup("[Red Underline]" + "Error! Can not start a task without clocking in.[/] Please clock in and try again.");
                return -1;
            }

            taskQueries.CreateNewTask(currentTask);

            if (settings.EndTime == null)
            {
                AnsiConsole.Markup("[green underline]" + $"Tracking Task[/]: {taskDescription}");
            }
            else
            {
                AnsiConsole.Markup("[green underline]" + $"Logging Task[/]: {taskDescription}");
            }

            return 0;
        }
    }
}
