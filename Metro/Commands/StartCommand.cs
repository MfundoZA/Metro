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
            var startTime = TimeOnly.FromDateTime(DateTime.Now);
            Task currentTask;

            var currentWorkDay = TextFileReader.ReadAllAsList<WorkDay>("workdays.json")?.Last();
            currentTask = new Task(taskDescription, startTime, currentWorkDay);

            TextFileWriter.Write(currentTask.ToString(), "Tasks.json");

            Console.WriteLine($"Tracking Task: {taskDescription}");
            return 0;
        }
    }
}
