using System.Net.Sockets;
using System.Text;
using MachineConnector.Config;
using MachineConnector.Contracts;

namespace MachineConnector.Rotox;

public class RotoxReceiver : IPartMessageReceiver
{
    private readonly RotoxMachine _rotoxMachine;
    private readonly IPartWorker _partWorker;
    private readonly ILogger<RotoxReceiver> _logger;
    private TcpListener _tcpListener = null!;
    private readonly object _lockObject = new();
    private static NetworkStream _networkStream = null!;

    public bool IsRunning { get; private set; }

    public RotoxReceiver(RotoxMachine rotoxMachine, IPartWorker partWorker, ILogger<RotoxReceiver> logger)
    {
        _rotoxMachine = rotoxMachine;
        _partWorker = partWorker;
        _logger = logger;
    }

    public async Task StartReceiving(CancellationToken cancelToken)
    {
        lock (_lockObject)
        {
            IsRunning = true;
        }

        await WaitForConnection(cancelToken);
    }

    private async Task WaitForConnection(CancellationToken cancellationToken)
    {
        try
        {
            _tcpListener = TcpListener.Create(_rotoxMachine.Port);
            _tcpListener.Start();
            while (!cancellationToken.IsCancellationRequested)
            {
                var client = await _tcpListener.AcceptTcpClientAsync(cancellationToken);
                var rotoxMessageReceiver = new RotoxMessageReceiver(client, _partWorker, _logger, cancellationToken);
                var thread = new Thread(rotoxMessageReceiver.OnClientConnection);
                thread.Start();
            }
            _tcpListener.Stop();
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Listening to port {RotoxMachinePort} could not started!", _rotoxMachine.Port);
        }
        finally
        {
            lock (_lockObject)
            {
                IsRunning = false;
            }
        }
    }

    private class RotoxMessageReceiver
    {
        private readonly TcpClient _client;
        private readonly IPartWorker _partWorker;
        private readonly ILogger<RotoxReceiver> _logger;
        private readonly CancellationToken _cancellationToken;
        private byte[] _buffer = null!;

        public RotoxMessageReceiver(TcpClient client, IPartWorker partWorker, ILogger<RotoxReceiver> logger, CancellationToken cancellationToken)
        {
            _client = client;
            _partWorker = partWorker;
            _logger = logger;
            _cancellationToken = cancellationToken;
        }
        
        public void OnClientConnection()
        {
            try
            {
                _buffer = new byte[_client.ReceiveBufferSize];
                _networkStream = _client.GetStream();
                GetMessages();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Accepting connection failed!");
            }
        }

        private void GetMessages()
        {
            while (!_cancellationToken.IsCancellationRequested)
                _networkStream.BeginRead(_buffer, 0, _buffer.Length, OnReceiveMessage, _cancellationToken);
        }

        private void OnReceiveMessage(IAsyncResult result)
        {
            try
            {
                if (_networkStream.CanRead)
                {
                    var length = _networkStream.EndRead(result);
                    var message = Encoding.ASCII.GetString(_buffer, 0, length);
                    SendMessage(message);
                }
                GetMessages();
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error in {nameof(OnReceiveMessage)}");
            }
        }

        private void SendMessage(string message)
        {
            var messageParts = message.Split(";");
            if (messageParts is {Length: >= 4})
            {
                var machineId = messageParts[2];
                var barcode = messageParts[3];
                if (messageParts[0].ToUpper() == "S")
                    _partWorker.StartPart(barcode, machineId);
                else if (messageParts[0].ToUpper() == "F")
                    _partWorker.FinishPart(barcode, machineId);
            }
        }
    }
}