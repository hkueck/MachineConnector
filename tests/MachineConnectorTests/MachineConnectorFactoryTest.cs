using FluentAssertions;
using MachineConnector;
using MachineConnector.Config;
using MachineConnector.Contracts;
using Moq;

namespace MachineConnectorTests;

public class MachineConnectorFactoryTest
{
    private IMachineConnectorFactory _machineConnectorFactory;
    private readonly Mock<IGetMachineConnectorConfigUseCase> _configUseCase;
    private readonly string _filePath = Path.GetTempPath();

    public MachineConnectorFactoryTest()
    {
        _configUseCase = new Mock<IGetMachineConnectorConfigUseCase>();
        var configPath = new Mock<IMachineConnectorConfigPath>();
        configPath.SetupGet(cp => cp.ConfigPath).Returns(_filePath);
        _machineConnectorFactory = new MachineConnectorFactory(_configUseCase.Object, configPath.Object);
    }

    [Fact]
    public void ConstructorShouldInitMember()
    {
        _machineConnectorFactory.Should().NotBeNull();
    }

    [Fact]
    public void InitShouldReadConfig()
    {
        _configUseCase.Setup(uc => uc.Execute()).Returns(new MachineConnectorConfiguration());
        
        _machineConnectorFactory.Init();

        _configUseCase.Verify(u => u.Execute());
    }

    [Fact]
    public void ConfigChangeShouldReadConfig()
    {
        _configUseCase.Setup(uc => uc.Execute()).Returns(new MachineConnectorConfiguration());
        _machineConnectorFactory.Init();

        var fileName = GetFileName(_filePath);
        File.WriteAllText(fileName, "config");

        Thread.Sleep(20);
        _configUseCase.Verify(u => u.Execute(), Times.Exactly(2));
    }

    private static string GetFileName(string path)
    {
        return Path.Combine(path, $"{nameof(MachineConnectorConfiguration)}.json");
    }
}
