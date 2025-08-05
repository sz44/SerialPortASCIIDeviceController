namespace SerialPortASCIIDeviceController;

public class ConsoleUI
{
    private CommandProcessor CommandProcessor;

    public ConsoleUI(CommandProcessor commandProccesor)
    {
        CommandProcessor = commandProccesor;
    }

    public void Start()
    {
        Console.WriteLine("Console App Started. Type 'quit' to exit.");
        while (true)
        {
            string? input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                continue;
            }
            if (input.Trim().ToLower() == "quit")
            {
                break;
            }

            CommandProcessor.ProcessCommand(input);
        }
    }
}
