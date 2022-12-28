namespace MachineConnector.Contracts;

public interface IPartWorker
{
    void StartPart(string barcode, string machineId);
    void FinishPart(string barcode, string machineId);
}