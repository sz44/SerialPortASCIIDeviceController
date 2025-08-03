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
    var deviceMap = JsonSerializer.Deserialize<DeviceConfig>(json, options);
    if (deviceMap == null)
    {
        return;
    }
    foreach (var (deviceName, device) in deviceMap)
    {
        device.CreateConnection();
        Console.WriteLine(device.Connection.PortName);
        /*connections[deviceName] = conn;*/
        /*dispatchTable[deviceName] = device.Commands;*/
    }
}
init();

/*class DeviceRegistry {*/
/*    private final Map<String, Device> devices = new HashMap<>();*/
/**/
/*    public void register(String deviceId, Device device) {*/
/*        devices.put(deviceId, device);*/
/*    }*/
/**/
/*    public Device getDevice(String deviceId) {*/
/*        return devices.get(deviceId);*/
/*    }*/
/*}*/

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

public class DeviceConfig : Dictionary<string, Device> { }

public class Device
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
