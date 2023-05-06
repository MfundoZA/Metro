using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Commands
{
    public class ReportCommand : Command<ReportSettings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] ReportSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
