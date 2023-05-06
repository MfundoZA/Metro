using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Commands
{
    public class ReportSettings : CommandSettings
    {
        [CommandArgument(0, "[timeFrame]")]
        public int timeFrame { get; set; }
    }
}
