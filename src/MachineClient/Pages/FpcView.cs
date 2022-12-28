using MachineClient.Model;

namespace MachineClient.Pages;

public partial class FpcView
{
    private Fpc[]? _fpcs;

    protected override void OnInitialized()
    {
        _fpcs = new[]
        {
            new Fpc {Name = "Keine Schramme", Type = "ja/nein", PartName = "Part 1"}, new Fpc {Name = "Keine Schramme", Type = "ja/nein", PartName = "Part 2"},
            new Fpc {Name = "Keine Schramme", Type = "ja/nein", PartName = "Part 3"}, new Fpc {Name = "Maschine reinigen", Type = "Wartung", PartName = ""}
        };
        base.OnInitialized();
    }
}