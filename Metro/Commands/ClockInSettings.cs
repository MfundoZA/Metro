using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Metro.Commands
{
    /// <summary>
    /// Class <c>ClockInSettings</c> holds the command line arguments and options for the <c>ClockInCommand</c>.
    /// 
    /// Params
    /// *  time - Optional, the time the user in clocking in. If not given the system assumes the current time when the user runs the command.
    /// *  force (-f) - Optional, will forcefully update (without prompting user) user's clock in time for the current day if one already exists. 
    /// </summary>
    public class ClockInSettings : CommandSettings
    {
        [CommandArgument(0, "[time]")]
        public string? Time { get; set; }

        [CommandOption("-f|--force")]
        public bool? Force { get; set; }
    }
}
