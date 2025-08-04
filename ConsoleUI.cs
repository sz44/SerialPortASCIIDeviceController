namespace SerialPortASCIIDeviceController;

public class ConsoleUI
{
    private CommandProcessor CommandProcessor;
    private CancellationTokenSource CancellationTokenSource;
    private bool isRunning = false;

    public ConsoleUI(CommandProcessor commandProccesor)
    {
        CommandProcessor = commandProccesor;
        CancellationTokenSource = new CancellationTokenSource();
    }

    public async Task Start()
    {
        if (isRunning)
        {
            Console.WriteLine("Console is already running.");
            return;
        }

        try
        {
            isRunning = true;
            Console.WriteLine("Console App Started. Type 'quit' to exit.");
            Console.Write("Command: ");

            // Start the input loop asynchronously
            await InputLoopAsync(CancellationTokenSource.Token);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting console: {ex.Message}");
        }
    }

    private async Task InputLoopAsync(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested && isRunning)
            {
                // Use Task.Run to make Console.ReadLine non-blocking
                string input = await Task.Run(() => Console.ReadLine(), cancellationToken);

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.Write("Command: ");
                    continue;
                }

                // Check for quit command
                if (input.Trim().ToLower() == "quit")
                {
                    await EndAsync();
                    break;
                }

                // Send command to serial port and handle response
                CommandProcessor.ProcessCommand(input);

                if (isRunning) // Only show prompt if still running
                {
                    Console.Write("Command: ");
                }
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("\nInput loop cancelled.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in input loop: {ex.Message}");
        }
    }

    public async Task EndAsync()
    {
        if (!isRunning)
        {
            Console.WriteLine("Console is not running.");
            return;
        }

        Console.WriteLine("Shutting down console...");
        isRunning = false;

        // Cancel the input loop
        CancellationTokenSource.Cancel();

        // Close serial port

        // Clean up
        /*serialPort?.Dispose();*/
        CancellationTokenSource?.Dispose();

        Console.WriteLine("Console shutdown complete.");
    }

    // Alternative method to end from external code
    public void Stop()
    {
        Task.Run(async () => await EndAsync());
    }
}
