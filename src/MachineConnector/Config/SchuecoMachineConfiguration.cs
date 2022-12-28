namespace MachineConnector.Config;

public class SchuecoMachineConfiguration
{
    public string Name { get; set; } = "Schueco Machines";
    public string ApiUrl { get; set; } = "localhost";
    public int ApiPort { get; set; } = 31750;
    public string NotificationUrl { get; set; } = "localhost";
    public int NotificationPort { get; set; } = 7001;
}