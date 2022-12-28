namespace MachineConnector.Config;

public class MachineConnectorConfiguration
{
    public SchuecoMachineConfiguration? SchuecoMachineConfiguration { get; set; }
    public RotoxMachineConfiguration RotoxMachineConfiguration { get; set; } = new();
}