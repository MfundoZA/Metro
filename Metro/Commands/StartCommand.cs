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
        private const string FILE_NAME = "tasks.json";

        public override int Execute([NotNull] CommandContext context, [NotNull] StartSettings settings)
        {
            var taskDescription = settings.Description;
            var startTime = DateTime.Now;
            Task currentTask;

            var currentWorkDay = TextFileReader.ReadAllAsList<WorkDay>("workdays.json")?.Where(x => x.ClockInTime.Date == DateTime.Today.Date).FirstOrDefault();
            List<Task>? currentTasks;

            /* Technical Debt */
            if (currentWorkDay != null)
            {
                currentTasks = TextFileReader.ReadAllAsList<Task>("tasks.json")?.Where(x => x.WorkDayId == currentWorkDay.Id).ToList();
                currentTask = new Task(taskDescription, startTime, currentWorkDay.Id);

                if (currentTasks == null)
                {
                    currentTasks = new List<Task>();
                }

                currentTasks.Add(currentTask);
            }
            else
            {
                Console.Error.WriteLine("Error! Can not start a task without clocking in. Please clock in and try again.");
                return -1;
            }

            using (var fileStream = new FileStream(FILE_NAME, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(fileStream, currentTasks);
            }

            Console.WriteLine($"Tracking Task: {taskDescription}");
            return 0;
        }
    }
}
