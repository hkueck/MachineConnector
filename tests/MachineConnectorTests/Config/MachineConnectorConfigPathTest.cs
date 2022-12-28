using System.Runtime.InteropServices;
using FluentAssertions;
using MachineConnector.Config;

namespace MachineConnectorTests.Config;

public class MachineConnectorConfigPathTest
{
    [Fact]
    public void ConstructorShouldInitMember()
    {
        var expectedPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Ofcas.MES", "MachineConnector");
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            expectedPath = Path.Combine(@"/Users/Shared", "Ofcas.MES", "MachineConnector");

        var configPath = new MachineConnectorConfigPath();

        configPath.ConfigPath.Should().Be(expectedPath);
    }
}