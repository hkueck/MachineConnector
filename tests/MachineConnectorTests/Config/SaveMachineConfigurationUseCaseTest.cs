using FluentAssertions;
using MachineConnector.Config;
using MachineConnector.Contracts;
using MachineConnector.UseCases;
using Moq;

namespace MachineConnectorTests.Config;

public class SaveMachineConfigurationUseCaseTest
{
    private readonly ISaveMachineConfigurationUseCase _useCase;
    private readonly Mock<IMachineConnectorConfigPath> _configPath;

    public SaveMachineConfigurationUseCaseTest()
    {
        _configPath = new Mock<IMachineConnectorConfigPath>();
        _useCase = new SaveMachineConfigurationUseCase(_configPath.Object);
    }

    [Fact]
    public void ConstructorShouldInitMember()
    {
        _useCase.Should().NotBeNull();
    }

    [Fact]
    public void ExecuteShouldSaveConfigFile()
    {
        var path = Path.Combine(Path.GetTempPath(), $"{nameof(MachineConnectorConfiguration)}.json");
        _configPath.SetupGet(cp => cp.ConfigPath).Returns(Path.GetTempPath());
        var config = new MachineConnectorConfiguration();

        _useCase.Execute(config);

        File.Exists(path).Should().BeTrue();
    }
}