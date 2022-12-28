using FluentAssertions;
using MachineConnector.Config;

namespace MachineConnectorTests.Config;

public class MachineConnectorConfigurationTests
{
    [Fact]
    public void ConstructorShouldInitMembers()
    {
        var configuration = new MachineConnectorConfiguration();
        configuration.Should().NotBeNull();
        configuration.SchuecoMachineConfiguration.Should().BeNull();
        configuration.RotoxMachineConfiguration.Should().NotBeNull();
    }
}
