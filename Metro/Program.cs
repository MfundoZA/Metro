using Metro.Commands;
using Metro.Models;
using Metro.Persistance;
using Spectre.Console.Cli;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Task = Metro.Models.Task;

var app = new CommandApp();

app.Configure(config =>
{
    config.AddCommand<ClockInCommand>("hi");
    config.AddCommand<ClockOutCommand>("bye");
    config.AddCommand<StartCommand>("Start");
    config.AddCommand<StopCommand>("Stop");
    config.AddCommand<LogCommand>("log");
    config.AddCommand<ReportCommand>("report");
});

return app.Run(args);