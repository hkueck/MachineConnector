using MachineConnector.Config;
using MachineConnector.Contracts;
using Microsoft.AspNetCore.Components;

namespace MachineConnector.Pages;

public partial class RotoxMachinesPage
{
    [Inject]
    public ISaveMachineConfigurationUseCase? SaveMachineConfigurationUseCase { get; set; }
    [Inject]
    private IGetMachineConnectorConfigUseCase? GetMachineConnectorConfigUseCase { get; set; }
    private List<RotoxMachine>? _rotoxMachines;
    private bool ShowDialog { get; set; }

    protected override void OnInitialized()
    {
        var configuration = GetMachineConnectorConfigUseCase?.Execute();
        _rotoxMachines = configuration?.RotoxMachineConfiguration.RotoxMachines;
    }

    private void OnAddMachine()
    {
        ShowDialog = true;
    }
    
    private void OnCloseAddDialog(RotoxMachine? machine)
    {
        if (machine != null)
        {
            _rotoxMachines?.Add(machine);
            var configuration = GetMachineConnectorConfigUseCase?.Execute();
            if (configuration != null)
            {
                configuration.RotoxMachineConfiguration.RotoxMachines = _rotoxMachines;
                SaveMachineConfigurationUseCase?.Execute(configuration);
            }
        }
        ShowDialog = false;
    }
}
