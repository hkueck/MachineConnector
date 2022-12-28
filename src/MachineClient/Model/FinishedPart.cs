namespace MachineClient.Model;

public class FinishedPart
{
    public string Name { get; set; } = "";
    public string Store { get; set; } = "";
    public int Pin { get; set; }
    public DateTime FinishTime { get; set; }
    public bool FpcNeeded { get; set; }
}
