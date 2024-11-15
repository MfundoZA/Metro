﻿using Metro.Models;
using Metro.Persistance;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Metro.Commands
{
    public class ClockInCommand : Command<ClockInSettings>
    {
        private const string TIME_FORMAT = "HH:MM";
        private const string FILE_NAME = "Workdays.json";

        public override int Execute([NotNull] CommandContext context, [NotNull] ClockInSettings settings)
        {
            DateTime clockInTime;
            WorkDay currentWorkday;

            if (settings.Time == null)
            {
                clockInTime = DateTime.Now;
            }
            else
            {
                if (DateTime.TryParse(settings.Time, out clockInTime) == false)
                {
                    AnsiConsole.Markup("[underline red]" + "Error! Time format is incorrect.[/] Please try again and ensure the format is as follows: " + TIME_FORMAT);
                    return -1;
                }
            }

            /* Technical Debt */
            var workdays = TextFileReader.ReadAllAsList<WorkDay>(FILE_NAME);

            var previousWorkday = workdays?.Last();

            if (previousWorkday != null)
            {
                currentWorkday = new WorkDay(previousWorkday.Id + 1, clockInTime);
            }
            else
            {
                currentWorkday = new WorkDay(1, clockInTime);
            }

            if (workdays != null && previousWorkday?.ClockInTime.Date == currentWorkday.ClockInTime.Date)
            {
                if (settings.Force == true || AnsiConsole.Confirm("Warning! You have already clocked in today. Do you want to override?"))
                {
                    workdays.Remove(workdays.Last());
                    workdays.Add(currentWorkday);
                }
            }
            else
            {
                if (workdays == null)
                {
                    workdays = new List<WorkDay>();
                }

                workdays.Add(currentWorkday);
            }

            using (var fileStream = new FileStream(FILE_NAME, FileMode.OpenOrCreate))
            {
                JsonSerializer.Serialize(fileStream, workdays);
            }

            AnsiConsole.Markup("Successfully clocked in @ [underline]" + currentWorkday.ClockInTime.ToShortTimeString() + "[/]");

            return 0;
        }
    }
}
