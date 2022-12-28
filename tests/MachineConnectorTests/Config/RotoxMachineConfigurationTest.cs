using FluentAssertions;
using MachineConnector.Config;

namespace MachineConnectorTests.Config;

public class RotoxMachineConfigurationTest
{
    [Fact]
    public void ConstructorShouldInitMember()
    {
        var configuration = new RotoxMachineConfiguration();

        configuration.RotoxMachines.Should().BeEmpty();
    }
}