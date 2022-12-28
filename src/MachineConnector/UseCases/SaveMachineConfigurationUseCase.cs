using MachineConnector.Config;
using MachineConnector.Contracts;
using Newtonsoft.Json;

namespace MachineConnector.UseCases;

public class SaveMachineConfigurationUseCase : ISaveMachineConfigurationUseCase
{
    private readonly IMachineConnectorConfigPath _configPath;

    public SaveMachineConfigurationUseCase(IMachineConnectorConfigPath configPath)
    {
        _configPath = configPath;
    }

    public void Execute(MachineConnectorConfiguration config)
    {
        var file = Path.Combine(_configPath.ConfigPath, $"{nameof(MachineConnectorConfiguration)}.json");
        var json = JsonConvert.SerializeObject(config, Formatting.Indented);
        File.WriteAllText(file, json);
    }
}