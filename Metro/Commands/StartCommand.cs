using Metro.Models;
using Metro.Persistance;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task = Metro.Models.Task;

namespace Metro.Commands
{
    public class StartCommand : Command<StartSettings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] StartSettings settings)
        {
            var taskDescription = settings.Description;
            var startTime = DateTime.Now;
            Task currentTask;

            var currentWorkDay = TextFileReader.ReadAllAsList<WorkDay>("workdays.json")?.Where(x => x.ClockInTime.Date == DateTime.Today.Date).FirstOrDefault();

            if (currentWorkDay != null)
            {
                currentTask = new Task(taskDescription, startTime, currentWorkDay);
            }
            else
            {
                Console.Error.WriteLine("Error! Can not start a task without clocking in. Please clock in and try again.");
                return -1;
            }

            TextFileWriter.Write(currentTask.ToString(), "Tasks.json");

            Console.WriteLine($"Tracking Task: {taskDescription}");
            return 0;
        }
    }
}
