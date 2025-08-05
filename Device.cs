namespace SerialPortASCIIDeviceController;

public class DeviceMap : Dictionary<string, Device> { }

// constucted from JSON deserialization
public class Device
{
    public required string Id { get; set; }
    public required string ConnectionType { get; set; }
    public required ConnectionConfig ConnectionConfig { get; set; }
    private MockSerialConnection? Connection;

    public void CreateConnection()
    {
        Connection = new MockSerialConnection(ConnectionConfig);
        Connection.DataReceived += OnDataReceived;
    }
    public void Connect()
    {
        if (Connection == null) { return; }
        Connection.Open();
    }
    public void Write(string msg)
    {
        if (Connection == null) { return; }
        Connection.Write(msg);
    }
    public void Read()
    {
        /*if (Connection == null) { return; }*/
        /*Connection.ReadLine();*/
    }
    /*private void OnLineReceived(object? sender, DataLineReceivedEventArgs e)*/
    /*{*/
    /*    Console.WriteLine($"[{e.Timestamp:HH:mm:ss.fff}] [{e.DeviceId}] Complete line: {e.LineText}");*/
    /*}*/
    private void OnDataReceived(string data)
    {
        Console.WriteLine($"Device {Id} received: {data}");
    }
}
