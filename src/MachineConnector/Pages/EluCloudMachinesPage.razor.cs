using MachineConnector.Model;

namespace MachineConnector.Pages;

public partial class EluCloudMachinesPage
{
    private EluCloudMachine[]? _eluCloudMachines;

    protected override void OnInitialized()
    {
        _eluCloudMachines = new []{new EluCloudMachine{Id = "1", Name = "SBZ621"}, new EluCloudMachine{Id = "2", Name = "SBZ628"}};
    }
}
