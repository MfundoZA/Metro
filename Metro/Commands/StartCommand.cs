using Metro.Models;
using Metro.Persistance;
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
        private const string FILE_NAME = "Tasks.json";
        private const string TIME_FORMAT = "HH:MM";

        public override int Execute([NotNull] CommandContext context, [NotNull] StartSettings settings)
        {
            string taskDescription = settings.Description;
            DateTime startTime;
            DateTime endTime;
            Task currentTask;

            var currentWorkDay = TextFileReader.ReadAllAsList<WorkDay>("Workdays.json")?.Where(x => x.ClockInTime.Date == DateTime.Today.Date).FirstOrDefault();
            List<Task>? currentTasks;

            /* Technical Debt */
            if (currentWorkDay != null)
            {
                currentTasks = TextFileReader.ReadAllAsList<Task>("Tasks.json")?.Where(x => x.WorkDayId == currentWorkDay.Id).ToList();

                if (currentTasks == null)
                {
                    currentTasks = new List<Task>();
                }

                var lastTask = TextFileReader.ReadAllAsList<Task>("Tasks.json")?.Last();

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
                    if (DateTime.TryParse(settings.EndTime, out endTime) == false)
                    {
                        AnsiConsole.Markup("[underline red]" + "Error! End time format is incorrect.[/] Please try again and ensure the format is as follows: " + TIME_FORMAT);
                        return -1;
                    }
                }
                else
                {
                    endTime = DateTime.Now;
                }

                if (lastTask != null)
                {
                    currentTask = new Task(lastTask.Id + 1, taskDescription, startTime, endTime, currentWorkDay.Id);
                }
                else
                {
                    currentTask = new Task(1, taskDescription, startTime, endTime, currentWorkDay.Id);
                }

                currentTasks.Add(currentTask);
            }
            else
            {
                AnsiConsole.Markup("[Red Underline]" + "Error! Can not start a task without clocking in.[/] Please clock in and try again.");
                return -1;
            }

            using (var fileStream = new FileStream(FILE_NAME, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(fileStream, currentTasks);
            }

            if (settings.EndTime == null)
            {
                Console.WriteLine($"Tracking Task: {taskDescription}");
            }
            else
            {
                Console.WriteLine($"Logging Task: {taskDescription}");
            }

            return 0;
        }
    }
}
