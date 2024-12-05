using Metro.Commands;
using Metro.Models;
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
    config.AddCommand<StartCommand>("start");
    config.AddCommand<StopCommand>("stop");
    config.AddCommand<ReportCommand>("report");
});

return app.Run(args);