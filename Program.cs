// See https://aka.ms/new-console-template for more information
using System.Collections.Concurrent;
using System.Text.Json;
/*using System.IO.Ports;*/


void init()
{
    var options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };


    string json = File.ReadAllText("devices.json");
    var deviceMap = JsonSerializer.Deserialize<DeviceMap>(json, options);
    if (deviceMap == null) { return; }


    foreach (var (deviceName, device) in deviceMap)
    {
        device.CreateConnection();
        /*Console.WriteLine(device.Connection.PortName);*/
        device.Connect();
        /*connections[deviceName] = conn;*/
        /*dispatchTable[deviceName] = device.Commands;*/
    }

    var consoleUI = new ConsoleUI();
}
init();

public class ConsoleUI
{
    public ConsoleUI()
    {
    }
    public void start()
    {
        Console.Write("Command:");
        while (true)
        {
            string input = Console.ReadLine();
        }
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

public abstract class Device
{
    public required string Id { get; set; }
    public required string Connection_Type { get; set; }
    public required ConnectionConfig Connection_Config { get; set; }
    public required Dictionary<string, string> Commands { get; set; }
    public SerialConnection? Connection { get; set; }

    public void CreateConnection()
    {
        Connection = new SerialConnection(Connection_Config);
    }
    public void Connect()
    {
        if (Connection == null) { return; }
        Connection.OpenPort();
    }
}

public class Device001 : Device
{
    public void MoveUp() { }
    public void MoveDown() { }
    public void GetStatus() { }
}

public class Device002 : Device
{
    public void SetVacuumOn() { }
    public void SetVacuumOff() { }
    public void SetAngle(int value) { }
    public void GetStatus() { }
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
    public void Write(byte[] request) { }
    public string Read()
    {
        return "Mock Device Read Response";
    }
}
