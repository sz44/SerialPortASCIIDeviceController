namespace SerialPortASCIIDeviceController;

public class ConnectionConfig
{
    public int Port { get; set; }
    public int Baud { get; set; }
    public int Parity { get; set; }
    public int DataBits { get; set; }
    public int StopBits { get; set; }
}

public class MockSerialConnection
{
    public string PortName { get; set; }
    public int BaudRate { get; set; }
    public int Parity { get; set; }
    public int DataBits { get; set; }
    public int StopBits { get; set; }

    private readonly Queue<string> DataQueue = new();
    public event Action<string>? DataReceived;
    private bool _isRunning = false;

    public MockSerialConnection(ConnectionConfig config)
    {
        PortName = "COM" + config.Port;
        BaudRate = config.Baud;
        Parity = config.Parity;
        DataBits = config.DataBits;
        StopBits = config.StopBits;
    }
    public void Open()
    {
        _isRunning = true;
        Task.Run(() => StartReadingLoop()); ;
    }
    public void Close()
    {
        _isRunning = false;
    }
    public void Write(string request)
    {
        DataQueue.Enqueue($"Response to: {request}");
    }
    public void StartReadingLoop()
    {
        while (_isRunning)
        {
            if (DataQueue.Count > 0)
            {
                string data = DataQueue.Dequeue();

                // 2. Fire the event to notify subscribers
                DataReceived?.Invoke(data);
            }

            // Small delay to prevent busy waiting
            Thread.Sleep(100);
        }
    }
}

// Event args for received data
public class DataLineReceivedEventArgs : EventArgs
{
    public string DeviceId { get; }
    public string LineText { get; }
    public DateTime Timestamp { get; }

    public DataLineReceivedEventArgs(string deviceId, string lineText)
    {
        DeviceId = deviceId;
        LineText = lineText;
        Timestamp = DateTime.Now;
    }
}
