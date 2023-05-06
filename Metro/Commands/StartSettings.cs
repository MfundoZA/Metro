using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Commands
{
    public class StartSettings : CommandSettings
    {
        [CommandArgument(0, "<description>")]
        public string Description { get; set; } = null!;
    }
}
