// See https://aka.ms/new-console-template for more information
using System.Text.Json;
/*using System.IO.Ports;*/

var options = new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
};

Console.WriteLine("Hello, World!");
string json = File.ReadAllText("devices.json");
var deviceMap = JsonSerializer.Deserialize<DeviceConfig>(json, options);
var device001 = deviceMap.First();
Console.WriteLine(device001);
Console.WriteLine(device001.Value.Id);
/*Console.WriteLine(deviceMap.Keys.First());*/

public class DeviceConfig : Dictionary<string, Device> { }

public class Device
{
    public required string Id { get; set; }
    public required string Connection_Type { get; set; }
    public required ConnectionConfig Connection_Config { get; set; }
    public required Dictionary<string, string> Commands { get; set; }

    public SerialConnection CreateConnection()
    {
        return new SerialConnection(Connection_Config);
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
