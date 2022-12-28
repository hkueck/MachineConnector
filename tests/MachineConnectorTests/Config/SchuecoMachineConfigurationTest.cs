using FluentAssertions;
using MachineConnector.Config;

namespace MachineConnectorTests.Config;

public class SchuecoMachineConfigurationTest
{
    [Fact]
    public void ConstructorShouldInitMember()
    {
        var configuration = new SchuecoMachineConfiguration();
        
        configuration.Name.Should().Be("Schueco Machines");
        configuration.ApiPort.Should().Be(31750);
        configuration.ApiUrl.Should().Be("localhost");
        configuration.NotificationPort.Should().Be(7001);
        configuration.NotificationUrl.Should().Be("localhost");

    }
}