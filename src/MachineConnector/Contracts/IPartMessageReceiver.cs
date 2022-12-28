namespace MachineConnector.Contracts;

public interface IPartMessageReceiver
{
    Task StartReceiving(CancellationToken cancelToken);
}