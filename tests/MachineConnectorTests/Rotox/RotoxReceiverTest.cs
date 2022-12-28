using System.Net.Sockets;
using System.Text;
using FluentAssertions;
using MachineConnector.Config;
using MachineConnector.Contracts;
using MachineConnector.Rotox;
using Microsoft.Extensions.Logging;
using Moq;

namespace MachineConnectorTests.Rotox;

public class RotoxReceiverTest
{
    private readonly Mock<IPartWorker> _partWorker;
    private readonly RotoxMachine _rotoxMachine;
    private readonly RotoxReceiver _rotoxReceiver;
    private readonly Mock<ILogger<RotoxReceiver>> _receiverLogger;

    public RotoxReceiverTest()
    {
        _partWorker = new Mock<IPartWorker>();
        _rotoxMachine = new RotoxMachine{Id = "R1", Name = "Rotox1", Port = 0};
        _receiverLogger = new Mock<ILogger<RotoxReceiver>>();
        _rotoxReceiver = new RotoxReceiver(_rotoxMachine, _partWorker.Object, _receiverLogger.Object);
    }

    [Fact]
    public void ConstructorShouldInitMember()
    {
        _rotoxReceiver.Should().NotBeNull();
    }

    [Fact]
    public void StopReceivingShouldStopReceiving()
    {
        _rotoxMachine.Port = 9002;

        var tokenSource = new CancellationTokenSource();
        _rotoxReceiver.StartReceiving(tokenSource.Token);

        tokenSource.Cancel();
        Thread.Sleep(20);
        
        _rotoxReceiver.IsRunning.Should().BeFalse();
    }

    [Fact]
    public void StartReceivingShouldStartReceiving()
    {
        _rotoxMachine.Port = 9004;
        var tokenSource = new CancellationTokenSource();

        var startReceiving = _rotoxReceiver.StartReceiving(tokenSource.Token);

        _rotoxReceiver.IsRunning.Should().BeTrue();

        tokenSource.Cancel();
    }

    [Fact]
    public void ReceivingStartMessageShouldCallStartPart()
    {
        _rotoxMachine.Port = 9003;
        var tokenSource = new CancellationTokenSource();
        var message = "S;;R1;34343";
        var startReceiving = _rotoxReceiver.StartReceiving(tokenSource.Token);
        var tcpClient = new TcpClient("localhost", 9003);
        var networkStream = tcpClient.GetStream();
        var bytes = Encoding.ASCII.GetBytes(message);

        networkStream.Write(bytes, 0, bytes.Length);
        
        Thread.Sleep(1000);
        _partWorker.Verify(pw => pw.StartPart( It.Is<string>(bc => bc == "34343"), It.Is<string>(id => id == "R1")));
        tokenSource.Cancel();
    }
    
    [Fact]
    public void ReceivingFinishMessageShouldCallFinishPart()
    {
        _rotoxMachine.Port = 9005;
        var tokenSource = new CancellationTokenSource();
        var message = "F;;R1;34343";
        var startReceiving = _rotoxReceiver.StartReceiving(tokenSource.Token);
        var tcpClient = new TcpClient("localhost", 9005);
        var networkStream = tcpClient.GetStream();
        var bytes = Encoding.ASCII.GetBytes(message);

        networkStream.Write(bytes, 0, bytes.Length);
        
        Thread.Sleep(400);
        _partWorker.Verify(pw => pw.FinishPart( It.Is<string>(bc => bc == "34343"), It.Is<string>(id => id == "R1")));
        tokenSource.Cancel();
    }

    [Fact]
    public void ReceivingStartMessageFrom2PortsShouldCallStartPart()
    {
        var receiverLogger2 = new Mock<ILogger<RotoxReceiver>>();
        _rotoxMachine.Port = 9006;
        var tokenSource = new CancellationTokenSource();
        var message1 = "S;;R1;34343";
        var startReceiving = _rotoxReceiver.StartReceiving(tokenSource.Token);
        using var tcpClient1 = new TcpClient("localhost", 9006);
        using var networkStream1 = tcpClient1.GetStream();
        var rotoxMachine2 = new RotoxMachine{Id = "R2", Name = "Rotox2", Port = 9007};
        var rotoxReceiver2 = new RotoxReceiver(rotoxMachine2, _partWorker.Object, receiverLogger2.Object);
        var message2 = "S;;R2;34344";
        var receiving = rotoxReceiver2.StartReceiving(tokenSource.Token);
        using var tcpClient2 = new TcpClient("localhost", 9007);
        using var networkStream2 = tcpClient2.GetStream();
        
        var bytes = Encoding.ASCII.GetBytes(message1);
        networkStream1.Write(bytes, 0, bytes.Length);
        bytes = Encoding.ASCII.GetBytes(message2);
        networkStream2.Write(bytes, 0, bytes.Length);
        
        Thread.Sleep(200);
        _partWorker.Verify(pw => pw.StartPart( It.Is<string>(bc => bc == "34343"), It.Is<string>(id => id == "R1")));
        _partWorker.Verify(pw => pw.StartPart( It.Is<string>(bc => bc == "34344"), It.Is<string>(id => id == "R2")));
        tokenSource.Cancel();
    }

    [Fact]
    public void StartReceivingWithWrongPortShouldWriteLog()
    {
        var tokenSource = new CancellationTokenSource();
        _rotoxMachine.Port = -1;

        var startReceiving = _rotoxReceiver.StartReceiving(tokenSource.Token);

        Thread.Sleep(10);
        _rotoxReceiver.IsRunning.Should().BeFalse();
        _receiverLogger.VerifyError<RotoxReceiver, Exception>("Listening to port -1 could not started!");
    }

    [Fact]
    public void ReceivingStartAndFinishMessageShouldCallStartPartAndFinishPart()
    {
        _rotoxMachine.Port = 9003;
        var tokenSource = new CancellationTokenSource();
        var startMessage = "S;;R1;34343";
        var finishMessage = "F;;R1;34343";
        var startReceiving = _rotoxReceiver.StartReceiving(tokenSource.Token);
        Thread.Sleep(10);
        var tcpClient = new TcpClient("localhost", 9003);
        var networkStream = tcpClient.GetStream();
        
        var bytes = Encoding.ASCII.GetBytes(startMessage);
        networkStream.Write(bytes, 0, bytes.Length);
        Thread.Sleep(10);

        bytes = Encoding.ASCII.GetBytes(finishMessage);
        networkStream.Write(bytes, 0, bytes.Length);

        Thread.Sleep(10);

        _partWorker.Verify(pw => pw.StartPart( It.Is<string>(bc => bc == "34343"), It.Is<string>(id => id == "R1")));
        _partWorker.Verify(pw => pw.FinishPart( It.Is<string>(bc => bc == "34343"), It.Is<string>(id => id == "R1")));
        tokenSource.Cancel();
        tcpClient.Close();
        tcpClient.Dispose();
        networkStream.Dispose();
    }
    
    [Fact]
    public void CloseClientAndStartNewClientShouldReceiveMessages()
    {
        _rotoxMachine.Port = 9008;
        var tokenSource = new CancellationTokenSource();
        var startMessage = "S;;R1;34343";
        var finishMessage = "F;;R1;34343";
        var startReceiving = _rotoxReceiver.StartReceiving(tokenSource.Token);
        Thread.Sleep(10);
        var tcpClient = new TcpClient("localhost", 9008);
        var networkStream = tcpClient.GetStream();
        
        var bytes = Encoding.ASCII.GetBytes(startMessage);
        networkStream.Write(bytes, 0, bytes.Length);
        Thread.Sleep(10);

        tcpClient.Close();
        networkStream.Dispose();
        Thread.Sleep(1000);
        
        tcpClient = new TcpClient("localhost", 9008);
        networkStream = tcpClient.GetStream();
        bytes = Encoding.ASCII.GetBytes(finishMessage);
        networkStream.Write(bytes, 0, bytes.Length);

        Thread.Sleep(1000);

        _partWorker.Verify(pw => pw.StartPart( It.Is<string>(bc => bc == "34343"), It.Is<string>(id => id == "R1")));
        _partWorker.Verify(pw => pw.FinishPart( It.Is<string>(bc => bc == "34343"), It.Is<string>(id => id == "R1")));
        tokenSource.Cancel();
        tcpClient.Close();
        tcpClient.Dispose();
        networkStream.Dispose();
    }
}

public static class LoggerMockExtension
{
    public static void VerifyError<T, TException>(this Mock<ILogger<T>> logger, string expectedMessage) where TException: Exception
    {
        Func<object, Type, bool> state = (v, t) => v.ToString()?.CompareTo(expectedMessage) == 0;
        logger.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Error),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => state(v,t)),
                It.IsAny<TException>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)));
    }
}