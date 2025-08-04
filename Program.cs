// See https://aka.ms/new-console-template for more information
using System.Collections.Concurrent;
using System.Text.Json;
/*using System.IO.Ports;*/


//////////////////// PROGRAM START /////////////////////////////
var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};


try
{
    string json = File.ReadAllText("devices.json");
    var deviceMap = JsonSerializer.Deserialize<DeviceMap>(json, options);
    if (deviceMap == null)
    {
        Console.WriteLine($"JSON deserialized into null");
        return;
    }

    foreach (var (deviceName, device) in deviceMap)
    {
        device.CreateConnection();
        device.Connect();
    }

    var commandProcessor = new CommandProcessor(deviceMap);

    var consoleUI = new ConsoleUI(commandProcessor);
    await consoleUI.Start();
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
    return;
}


/////////////////////////////////////////////////////////////////
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
                await CommandProcessor.ProcessCommand(input);

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
}

public class CommandProcessor
{
    Dictionary<string, Device> DeviceMap;

    public CommandProcessor(Dictionary<string, Device> deviceMap)
    {
        DeviceMap = deviceMap;
    }

    public void ProcessCommand(Command cmd)
    {
        var found = DeviceMap.TryGetValue(cmd.DeviceID, out Device device);
        if (!found)
        {
            Console.WriteLine("command failed, could not find deviceId");
            return;
        }
        var msg = cmd.Action + " " + cmd.Value;
        device.Connection.WriteString(msg);
    }
}

public class UserMessageQueue
{
    BlockingCollection<Message> Queue;
    public UserMessageQueue()
    {
        Queue = new BlockingCollection<Message>();
    }
    public void put(Message msg) { }
    public void get() { }
}

public class UserMessageProcessor
{
    public UserMessageProcessor()
    {

    }
}

public class Message
{
    public string Data { get; set; }

    public Message(string data)
    {
        Data = data;
    }
}

public class Command
{
    public string DeviceID { get; set; }
    public string Action { get; set; }
    public string Value { get; set; }

    public Command(string deviceID, string action, string value)
    {
        DeviceID = deviceID;
        Action = action;
        Value = value;
    }
}

public class DeviceMap : Dictionary<string, Device> { }

// constucted from JSON deserialization
public class Device
{
    public required string Id { get; set; }
    public required string ConnectionType { get; set; }
    public required ConnectionConfig ConnectionConfig { get; set; }
    private SerialConnection? Connection;

    public void CreateConnection()
    {
        Connection = new SerialConnection(ConnectionConfig);
    }
    public void Connect()
    {
        if (Connection == null) { return; }
        Connection.OpenPort();
    }
    public void Write(string msg)
    {
        if (Connection == null) { return; }
        Connection.WriteString(msg);
    }
    public void Read()
    {
        if (Connection == null) { return; }
        Connection.Read();
    }
}

public class ConnectionConfig
{
    public int Port { get; set; }
    public int Baud { get; set; }
    public int Parity { get; set; }
    public int DataBits { get; set; }
    public int StopBits { get; set; }
}

public class SerialConnection
{
    public string PortName { get; set; }
    public int BaudRate { get; set; }
    public int Parity { get; set; }
    public int DataBits { get; set; }
    public int StopBits { get; set; }
    public SerialConnection(ConnectionConfig config)
    {
        PortName = "COM" + config.Port;
        BaudRate = config.Baud;
        Parity = config.Parity;
        DataBits = config.DataBits;
        StopBits = config.StopBits;
    }
    public void OpenPort() { }
    public void ClosePort() { }
    public void Write(byte[] request)
    {
        Console.WriteLine($"wrote: {request}");
    }
    public void WriteString(string request)
    {
        Console.WriteLine($"wrote: {request}");
    }
    public string Read()
    {
        return "Mock Device Read Response";
    }
}
