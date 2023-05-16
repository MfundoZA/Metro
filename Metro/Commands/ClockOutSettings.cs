using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Commands
{
    public class ClockOutSettings : CommandSettings
    {
        [CommandArgument(0, "[time]")]
        public string? Time { get; set; }

        [CommandOption("-f|--force")]
        public bool? Force { get; set; }
    }
}
