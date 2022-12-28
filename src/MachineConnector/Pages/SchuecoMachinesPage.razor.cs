using MachineConnector.Model;

namespace MachineConnector.Pages;

public partial class SchuecoMachinesPage
{
    private SchuecoMachine[]? _schuecoMachines;

    protected override void OnInitialized()
    {
        _schuecoMachines = new []{new SchuecoMachine{Id = "1", Name = "Schueco1"}, new SchuecoMachine{Id = "2", Name = "Schueco2"}};
    }
}
