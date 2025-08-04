namespace SerialPortASCIIDeviceController;

public class CommandProcessor
{
    Dictionary<string, Device> DeviceMap;

    public CommandProcessor(Dictionary<string, Device> deviceMap)
    {
        DeviceMap = deviceMap;
    }

    public Command BuildCommand(string input)
    {
        var res = new List<string>();

        for (int i = 0; i < 2; i++)
        {
            /*int index = input.IndexOf(' ');*/
            var parts = input.Split(new[] { ' ' }, 2);
            res.Append(parts[0]);
            input = parts[1];
        }
        return new Command(res[0], res[1], input);
    }

    public void ProcessCommand(Command cmd)
    {
        var found = DeviceMap.TryGetValue(cmd.DeviceID, out Device? device);
        if (!found || device == null)
        {
            Console.WriteLine($"Command failed: could not find valid device for ID '{cmd.DeviceID}'");
            return;
        }
        var msg = cmd.Action + " " + cmd.Value;
        device.Write(msg);
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
