using FluentAssertions;
using MachineConnector.Contracts;
using MachineConnector.Rotox;

namespace MachineConnectorTests.Rotox;

public class RotoxReceiverCreatorTest
{
    private IRotoxReceiverCreator _receiverCreator;

    public RotoxReceiverCreatorTest()
    {
        _receiverCreator = new RotoxReceiverCreator();
    }
    [Fact]
    public void ConstructorShouldInitMember()
    {
        _receiverCreator.Should().NotBeNull();
    }

    [Fact]
    public void CreateShouldCreateReceivers()
    {
        
    }
}