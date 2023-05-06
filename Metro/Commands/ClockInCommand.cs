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
        public override int Execute([NotNull] CommandContext context, [NotNull] ClockInSettings settings)
        {
            var clockIn = DateTime.Now.ToString();
            var jsonData = JsonSerializer.Serialize(clockIn);

            TextFileWriter.Write(jsonData, "Workdays.json");
            return 0;
        }
    }
}
