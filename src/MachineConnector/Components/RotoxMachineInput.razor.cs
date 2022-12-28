using MachineConnector.Config;
using MachineConnector.Pages;
using Microsoft.AspNetCore.Components;
namespace MachineConnector.Components;

public partial class RotoxMachineInput
{
    private string _caption  = "Rotox Maschine";
    private string _message  = "Legen Sie eine neue Rotox Maschine an";
    [Parameter] 
    public RotoxMachine RotoxMachine { get; set; } = new RotoxMachine();
    [Parameter] 
    public EventCallback<RotoxMachine> OnClose { get; set; }
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string? _id = String.Empty;
    private string _name = String.Empty;
    private int _port;

    protected override void OnParametersSet()
    {
        _id = RotoxMachine.Id;
        _name = RotoxMachine.Name;
        _port = RotoxMachine.Port;

        base.OnParametersSet();
    }
    private void Cancel()
    {
        OnClose.InvokeAsync(arg: null);
    }
    private void Ok()
    {
        RotoxMachine.Id = _id;
        RotoxMachine.Name = _name;
        RotoxMachine.Port = _port;
        OnClose.InvokeAsync(RotoxMachine);
    }
}