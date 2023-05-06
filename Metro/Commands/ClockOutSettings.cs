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
        [CommandOption("-f|--force")]
        public bool? Force { get; set; }
    }
}
