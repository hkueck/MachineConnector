using MachineConnector.Config;
using MachineConnector.Contracts;
using Newtonsoft.Json;

namespace MachineConnector.UseCases;

public class GetMachineConnectorConfigUseCase : IGetMachineConnectorConfigUseCase
{
    private readonly IMachineConnectorConfigPath _configPath;

    public GetMachineConnectorConfigUseCase(IMachineConnectorConfigPath configPath)
    {
        _configPath = configPath;
    }

    public MachineConnectorConfiguration Execute()
    {
        var file = Path.Combine(_configPath.ConfigPath, $"{nameof(MachineConnectorConfiguration)}.json");
        if (!Directory.Exists(_configPath.ConfigPath))
        {
            Directory.CreateDirectory(_configPath.ConfigPath);
        }

        MachineConnectorConfiguration? configuration;
        if (File.Exists(file))
        {
            var json = File.ReadAllText(file);
            configuration = JsonConvert.DeserializeObject<MachineConnectorConfiguration>(json);
            if (configuration != null)
                return configuration;
        }

        configuration = new MachineConnectorConfiguration();
        var jsonw = JsonConvert.SerializeObject(configuration, Formatting.Indented);
        File.WriteAllText(file, jsonw);
        return configuration;
    }
}