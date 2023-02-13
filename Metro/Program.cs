var input = args[0];

while (input != "stop")
{

    switch (input.ToLower())
    {
        case "hi":
            // log date and time
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
}

Console.ReadKey();