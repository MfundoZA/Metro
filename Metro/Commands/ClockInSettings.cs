using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Metro.Commands
{
    public class ClockInSettings : CommandSettings
    {
        [CommandArgument(0, "[time]")]
        public string? Time { get; set; }

        [CommandOption("-f|--force")]
        public bool? Force { get; set; }
    }
}
