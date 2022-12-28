using MachineClient.Model;

namespace MachineClient.Pages;

public partial class Index
{
    private FinishedPart[]? _finishedParts;

    protected override void OnInitialized()
    {
        _finishedParts = new[]
        {
            new FinishedPart {Name = "Part 1", Store = "Lager 1", Pin = 1, FinishTime = DateTime.Now.AddMinutes(-1), FpcNeeded = true},
            new FinishedPart {Name = "Part 2", Store = "Lager 1", Pin = 1, FinishTime = DateTime.Now.AddMinutes(-2)},
            new FinishedPart {Name = "Part 3", Store = "Lager 1", Pin = 1, FinishTime = DateTime.Now.AddMinutes(-3), FpcNeeded = true}
        };
        base.OnInitialized();
    }
}