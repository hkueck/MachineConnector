using MachineConnector.Config;

namespace MachineConnector.Contracts;

public interface ISaveMachineConfigurationUseCase
{
    void Execute(MachineConnectorConfiguration config);
}