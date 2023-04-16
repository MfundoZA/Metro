using Metro.Persistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro.Commands
{
    public class ClockInCommand : Command
    {
        public DateTime TimeClockedIn { get; set; }

        public ClockInCommand()
        {
            TimeClockedIn = DateTime.Now;
        }

        public ClockInCommand(DateTime timeClockedIn)
        {
            TimeClockedIn= timeClockedIn;
        }

        public void Execute()
        {
            TextFileWriter.Write(TimeClockedIn, "Workdays.json");
        }
    }
}
