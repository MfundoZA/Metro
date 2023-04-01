using Metro.Models;
using Metro.Persistance;
using System.Globalization;
using System.Text;
using System.Text.Json;
using Task = Metro.Models.Task;

var input = args[0];

switch (input.ToLower())
{
    case "hi":
        // if user is already clocked in report current day
        if (isClockedIn())
        {
            Console.WriteLine("You're already clocked in! Here's your log for the day (use \"metero rpt\" whenever you would like to view again):");
            break;
        }

        {
            WorkDay currentWorkDay = new WorkDay();

            if (args.Length == 1)
            {
        // Welcome user to new day and log the date and
        // TODO Check if ChatGPT can do this
        currentWorkDay.ClockInTime = DateTime.Now;
            }
            else if (args.Length == 2 && args[1].Contains(':'))
            {
                var time = args[1].Split(':');

                currentWorkDay.ClockInTime = DateTime.Today.AddHours(Double.Parse(time[0])).AddMinutes(Double.Parse(time[1]));
            }

        TextFileWriter.Write(currentWorkDay, "Workdays.json");
         
        Console.WriteLine($"Good day! You are now clocked in at {currentWorkDay.ClockInTime}. Use ");
        }
        break;

    case "bye":
        // log last task duration


        // log clock off time
        var workDays = getAllWorkDays();

        if (workDays != null)
        {
            if (workDays.Last().ClockOutTime == null)
            {
                workDays.Last<WorkDay>().ClockOutTime = DateTime.Now;
            }
            else
            {
                // Get user confirmation to update ClockOutTime
            }
        }

        // save locally
        var stream = File.Open("Workdays.json", FileMode.Create);
        JsonSerializer.SerializeAsync(stream, workDays, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        stream.DisposeAsync();
        Console.WriteLine("Bye");
        break;

    case "start":
        var taskDescription = args[1];
        var startTime = TimeOnly.FromDateTime(DateTime.Now);
        Task currentTask;

        {
            WorkDay currentWorkDay = getAllWorkDays().Last();

            if (args.Length == 1)
            {
                Console.WriteLine("Error! A task must have a description! Please try again.");
                break;
            }
            else {
                currentTask = new Task(taskDescription, startTime, currentWorkDay);
        }
        }

        TextFileWriter.Write(currentTask.ToString(), "Tasks.json");

        Console.WriteLine($"Tracking Task: {taskDescription}");
        break;

    case "log":
    // Think about how task logging will work
    // Are tasks always serial? Obiviously not. So how do we keep
    // track of multiple task simultaneously? Maybe we can have it
    // so that a user will have to start a task in order to track
    // the time spent. A user might also be able to estimate
    // roughly how much time they spent on a task instead
        {
            string description;
            TimeOnly startTime;
            TimeOnly endTime;
            WorkDay workDay;
            Task newTask;

    // get minutes from clock in time or last task
            if (args.Length == 1)
            {
                Console.WriteLine("Error! A task must have a description! Please try again.");
            }
            else
            {
                if (args.Length == 3)
                {
                    description = args[1];
                    var stream = File.Open("Tasks.json", FileMode.Append);
                    startTime = (TimeOnly) JsonSerializer.DeserializeAsync<List<Task>>(stream, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    }).Result.Last().EndTime;
                    stream.DisposeAsync();
                    endTime = TimeOnly.FromDateTime(DateTime.Now);
                    workDay = getAllWorkDays().Last();

                    newTask = new Task(description, startTime, endTime, workDay);

                    TextFileWriter.Write(newTask, "Tasks.json");
                    
                }
            }

        }
        break;
    case "rpt":
        // display productivity for the last 30 days
        break;

    case "rpt-year":
        // display productivity for the last 12 months
        break;

    case "rpt-day":
        // display productivity for the last 24 hours
        break;

    case "help":
        // call method that will display all commands
        break;

    default:
        // call method that will dispaly all commands
        break;

}

bool isClockedIn()
{
    var workDays = getAllWorkDays();

    if (workDays == null || workDays.Count == 0)
    {
        return false;
    }
    else if (workDays.Last().ClockInTime.Date == DateTime.Now.Date)
    {
        return true; 
    }
    else
    {
        return false;
    }
}

List<WorkDay>? getAllWorkDays()
{
    string jsonInput;
    byte[] byteArray;
    MemoryStream inputStream;
    List<WorkDay>? workDays;

    try
    { 
        jsonInput = TextFileReader.ReadAll("Workdays.json");
        byteArray = Encoding.UTF8.GetBytes(jsonInput);
        inputStream = new MemoryStream(byteArray);

        workDays = JsonSerializer.DeserializeAsync<List<WorkDay>>(inputStream, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        }).Result;
    }
    catch (Exception ex) when (ex is JsonException || ex is FileNotFoundException)
    {
        workDays = null;
    }
    
    return workDays;
}