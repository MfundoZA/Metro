using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Commands
{
    public class StopSettings : CommandSettings
    {
        [CommandArgument(0, "[taskNumber]")]
        public int TaskNumber { get; set; }
    }
}
