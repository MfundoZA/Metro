using Metro.Models;
using Metro.Persistance;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
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
            DateTime? startTime;
            DateTime endTime;
            WorkDay? workDay;
            Task newTask;

            // get minutes from clock in time or last task
            description = settings.Description;
            var stream = File.Open("Tasks.json", FileMode.Append);

            if (settings.StartTime == null)
            {
                startTime = JsonSerializer.DeserializeAsync<List<Task>>(stream, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }).Result.Where(x => x.StartTime.Date == DateTime.Today.Date).Last().EndTime;

                // if startTime null due to there being no tasks on the current day then start time
                // is equal to the clock-in time of the day
                if (startTime.HasValue == false)
                {
                    startTime = TextFileReader.ReadAllAsList<WorkDay>("Workday.json")?.Where(x => x.ClockInTime.Date == DateTime.Today).FirstOrDefault().ClockInTime;
                }
            }
            else
            {
                startTime = DateTime.Parse(settings.StartTime);
            }

            stream.DisposeAsync();
            endTime = DateTime.Now;
            workDay = TextFileReader.ReadAllAsList<WorkDay>("Workday.json")?.Last();

            newTask = new Task(description, (DateTime) startTime, endTime, workDay.Id);

            TextFileWriter.Write(newTask, "Tasks.json");
            return 0;
        }
    }
}
