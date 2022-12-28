using FluentAssertions;
using MachineConnector.Config;
using MachineConnector.Contracts;
using MachineConnector.UseCases;
using Moq;

namespace MachineConnectorTests.Config;

public class GetMachineConnectorConfigUseCaseTest
{
    private Mock<IMachineConnectorConfigPath> _configPath;
    private IGetMachineConnectorConfigUseCase _useCase;

    public GetMachineConnectorConfigUseCaseTest()
    {
        _configPath = new Mock<IMachineConnectorConfigPath>();
        _useCase = new GetMachineConnectorConfigUseCase(_configPath.Object);
    }

    [Fact]
    public void ConstructorShouldInitMember()
    {
        _useCase.Should().NotBeNull();
    }

    [Fact]
    public void ExecuteShouldReturnSchuecoConfig()
    {
        var tempPath = Path.GetTempPath();
        WriteSchuecoConfig(GetFileName(tempPath));
        _configPath.SetupGet(cp => cp.ConfigPath).Returns(tempPath);

        var configuration = _useCase.Execute();

        configuration.SchuecoMachineConfiguration.Should().NotBeNull();
        configuration.SchuecoMachineConfiguration?.Name.Should().Be("new machines");
        configuration.SchuecoMachineConfiguration?.ApiPort.Should().Be(9000);
        configuration.SchuecoMachineConfiguration?.ApiUrl.Should().Be("192.168.1.1");
        configuration.SchuecoMachineConfiguration?.NotificationPort.Should().Be(9001);
        configuration.SchuecoMachineConfiguration?.NotificationUrl.Should().Be("192.168.1.2");
    }

    [Fact]
    public void ExecuteShouldReturnRotoxConfig()
    {
        var tempPath = Path.GetTempPath();
        WriteRotoxConfig(GetFileName(tempPath));
        _configPath.SetupGet(cp => cp.ConfigPath).Returns(tempPath);

        var configuration = _useCase.Execute();

        configuration.RotoxMachineConfiguration.Should().NotBeNull();
        for (int i = 0; i < 2; i++)
        {
            var rotoxMachine = configuration.RotoxMachineConfiguration?.RotoxMachines[i];
            rotoxMachine?.Id.Should().Be($"m{i}");
            rotoxMachine?.Name.Should().Be($"Rotox{i}");
            rotoxMachine?.Port.Should().Be(9000 + i);
        }
    }

    private void WriteRotoxConfig(string fileName)
    {
        var json = @"{""RotoxMachineConfiguration"": {
                        ""RotoxMachines"": [
                            {""Id"": ""m0"",
                            ""Name"": ""Rotox0"",
                            ""Port"": 9000 },
                            {""Id"": ""m1"",
                            ""Name"": ""Rotox1"",
                            ""Port"": 9001}
                        ]
                        }
                    }";
        File.WriteAllText(fileName, json);
    }

    private static string GetFileName(string path)
    {
        return Path.Combine(path, $"{nameof(MachineConnectorConfiguration)}.json");
    }

    private static void WriteSchuecoConfig(string fileName)
    {
        var json = @"{""SchuecoMachineConfiguration"": {
                ""Name"": ""new machines"",
                ""ApiUrl"": ""192.168.1.1"",
                ""ApiPort"": 9000,
                ""NotificationUrl"": ""192.168.1.2"",
                ""NotificationPort"": 9001
            }
        }";
        File.WriteAllText(fileName, json);
    }
}