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

namespace Metro.Commands
{
    public class ClockOutCommand : Command<ClockOutSettings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] ClockOutSettings settings)
        {

            // log clock off time

            // log last task duration
            var workDays = TextFileReader.ReadAllAsList<WorkDay>("Workdays.json");

            if (workDays != null)
            {
                if (workDays.Last().ClockOutTime == null)
                {
                    workDays.Last<WorkDay>().ClockOutTime = DateTime.Now;
                }
                else
                {
                    // Get user confirmation to update ClockOutTime
                }
            }

            // save locally
            var stream = File.Open("Workdays.json", FileMode.Create);
            JsonSerializer.SerializeAsync(stream, workDays, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            stream.DisposeAsync();
            Console.WriteLine("Bye");
            return 0;
        }
    }
}
