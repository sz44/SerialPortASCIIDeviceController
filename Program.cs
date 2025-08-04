using System.Text.Json;

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
