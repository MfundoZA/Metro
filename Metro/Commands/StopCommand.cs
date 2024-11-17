using Metro.Persistance;
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

namespace Metro.Commands
{
    public class StopCommand : Command<StopSettings>
    {
        private const string FILE_NAME = "Tasks.json";

        public override int Execute([NotNull] CommandContext context, [NotNull] StopSettings settings)
        {
            /* Technical Debt */
            // Pull all tasks and find tasks that have null end times
            List<WorkDay>? workDays = (List<WorkDay>?) TextFileReader.ReadAllAsList<WorkDay>(FILE_NAME);
            List<Task>? tasks = (List<Task>?) TextFileReader.ReadAllAsList<Task>(FILE_NAME)?.Where(x => x.StartTime.Date == DateTime.Today.Date).ToList();
            List<Task>? currentTasks = new List<Task>();
            List<string>? currentTasksDescriptions = new List<string>();
            List<string>? tasksToStop= new List<string>();

            if (workDays == null || workDays.Last<WorkDay>().ClockInTime == DateTime.Today.Date.AddDays(-1))
            {
                Console.Error.WriteLine("Error! You are currently not clocked in. Please clock in and try again.");
            }

            if (tasks != null)
            {

                foreach (var task in tasks)
                {
                    if (task.EndTime == null)
                    {
                        currentTasks.Add(task);
                        currentTasksDescriptions.Add(task.Description);
                    }
                }
            }
            else
            {
                Console.Error.WriteLine("Error! No tasks found. Please make sure you have tasks that need to be stopped and try again.");
                return -1;
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
                    tasks.Where(x => x.Description == taskDescription).First().EndTime = timeFinished;
                }

                using (var fileStream = new FileStream(FILE_NAME, FileMode.OpenOrCreate))
                {
                    JsonSerializer.Serialize(fileStream, tasks);
                }
            }

            return 0;
        }
    }
}
