using Metro.Models;
using Metro.Persistance;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Metro.Commands
{
    public class ClockInCommand : Command<ClockInSettings>
    {
        private const string FILE_NAME = "Workdays.json";

        public override int Execute([NotNull] CommandContext context, [NotNull] ClockInSettings settings)
        {
            var clockIn = DateTime.Now;
            var workday = new WorkDay(clockIn);

            var workdays = TextFileReader.ReadAllAsList<WorkDay>(FILE_NAME);
            workdays.Add(workday);

            using (var fileStream = new FileStream(FILE_NAME, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(fileStream, workdays);
            }

            return 0;
        }
    }
}
