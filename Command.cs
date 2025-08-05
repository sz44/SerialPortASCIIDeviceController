namespace SerialPortASCIIDeviceController;

public class CommandProcessor
{
    Dictionary<string, Device> DeviceMap;

    public CommandProcessor(Dictionary<string, Device> deviceMap)
    {
        DeviceMap = deviceMap;
    }

    public Command? BuildCommand(string input)
    {
        var parts = input.Split();

        if (parts.Length < 3)
        {
            Console.WriteLine("Not enought values. example: Device001 move 123");
            return null;
        }
        var values = parts[2..];
        return new Command(parts[0], parts[1], String.Join(" ", values));
    }

    public void ProcessCommand(string input)
    {
        Command? cmd = BuildCommand(input);
        if (cmd == null)
        {
            /*Console.WriteLine("Invalid command syntax.");*/
            return;
        }
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
