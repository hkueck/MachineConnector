using MachineConnector.Config;
using MachineConnector.Contracts;

namespace MachineConnector;

public class MachineConnectorFactory : IMachineConnectorFactory
{
    private readonly IGetMachineConnectorConfigUseCase _getConfigUseCase;
    private FileSystemWatcher? _configWatcher;
    private readonly string _configFile;
    private readonly string _configPath;

    public MachineConnectorFactory(IGetMachineConnectorConfigUseCase getConfigUseCase, IMachineConnectorConfigPath machineConnectorConfigPath)
    {
        _getConfigUseCase = getConfigUseCase;
        _configFile = $"{nameof(MachineConnectorConfiguration)}.json";
        _configPath = machineConnectorConfigPath.ConfigPath;
    }

    private void ConfigChanged(object sender, FileSystemEventArgs e)
    {
        _getConfigUseCase.Execute();
    }

    public void Init()
    {
        var configuration = _getConfigUseCase.Execute();

        ActivateConfigWatcher();
        CreateRotoxReceivers(configuration.RotoxMachineConfiguration);
    }

    private void CreateRotoxReceivers(RotoxMachineConfiguration? configurationRotoxMachineConfiguration)
    {
    }

    private void ActivateConfigWatcher()
    {
        _configWatcher = new FileSystemWatcher(_configPath);
        _configWatcher.NotifyFilter = NotifyFilters.LastWrite;
        _configWatcher.Changed += ConfigChanged;
        _configWatcher.Created += ConfigChanged;
        _configWatcher.Filter = _configFile;
        _configWatcher.EnableRaisingEvents = true;
    }
}