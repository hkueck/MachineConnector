using MachineConnector.Config;
using MachineConnector.Contracts;
using MachineConnector.UseCases;

namespace MachineConnector;

public static class MachineConnectorServiceExtension
{
    public static IServiceCollection AddMachineConnectorServices(this IServiceCollection services)
    {
        services.AddSingleton<IGetMachineConnectorConfigUseCase, GetMachineConnectorConfigUseCase>();
        services.AddSingleton<ISaveMachineConfigurationUseCase, SaveMachineConfigurationUseCase>();
        services.AddSingleton<IMachineConnectorConfigPath, MachineConnectorConfigPath>();
        services.AddSingleton<IMachineConnectorFactory, MachineConnectorFactory>();
        return services;
    }
}