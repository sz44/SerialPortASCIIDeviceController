namespace SerialPortASCIIDeviceController;

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
        Connection.Write(msg);
    }
    public void Read()
    {
        if (Connection == null) { return; }
        Connection.Read();
    }
}
