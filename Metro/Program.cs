var input = args[0];

switch (input.ToLower())
    {
        case "hi":
            // check if day is new
            // if true then log date and time

        // if user is already clocked in report current day
        if (isClockedIn())
            {
                Console.WriteLine("You're already clocked in! Here's your log for the day (use \"metero rpt\" whenever you would like to view again):");
            break;
            }

        // Welcome user to new day and log the date and
        // TODO Check if ChatGPT can do this
        var currentDateTime = DateTime.Now;
        Console.WriteLine($"Good day! You are now clocked in at {currentDateTime}. Use ");

            // save locally

            Console.WriteLine("Hi");
            break;

        case "bye":
            // log last task duration
            // log clock off time
            // save locally

            Console.WriteLine("Bye");
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

input = Console.ReadLine();

bool isClockedIn()
{
    throw new NotImplementedException();
}

Console.ReadKey();