namespace SerialPortASCIIDeviceController;

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
    public void Write(string request)
    {
        Console.WriteLine($"wrote: {request}");
    }
    public string Read()
    {
        return "Mock Device Read Response";
    }
}
