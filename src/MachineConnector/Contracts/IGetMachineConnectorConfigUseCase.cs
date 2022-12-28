using MachineConnector.Config;

namespace MachineConnector.Contracts;

public interface IGetMachineConnectorConfigUseCase
{
    MachineConnectorConfiguration Execute();
}