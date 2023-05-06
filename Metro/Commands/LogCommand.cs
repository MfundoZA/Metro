using Metro.Models;
using Metro.Persistance;
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
    public class LogCommand : Command<LogSettings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] LogSettings settings)
        {
            // Think about how task logging will work
            // Are tasks always serial? Obiviously not. So how do we keep
            // track of multiple task simultaneously? Maybe we can have it
            // so that a user will have to start a task in order to track
            // the time spent. A user might also be able to estimate
            // roughly how much time they spent on a task instead
            string description;
            TimeOnly endTime;
            WorkDay? workDay;
            Task newTask;

            // get minutes from clock in time or last task
            description = settings.Description;
            var stream = File.Open("Tasks.json", FileMode.Append);
            var startTime = (TimeOnly) JsonSerializer.DeserializeAsync<List<Task>>(stream, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }).Result.Last().EndTime;
            stream.DisposeAsync();
            endTime = TimeOnly.FromDateTime(DateTime.Now);
            workDay = TextFileReader.ReadAllAsList<WorkDay>("Workday.json")?.Last();

            newTask = new Task(description, startTime, endTime, workDay);

            TextFileWriter.Write(newTask, "Tasks.json");
            return 0;
        }
    }
}
