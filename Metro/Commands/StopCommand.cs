using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Commands
{
    public class StopCommand : Command<StopSettings>
    {
        public override int Execute([NotNull] CommandContext context, [NotNull] StopSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}
