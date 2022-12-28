using System.Runtime.InteropServices;
using MachineConnector.Contracts;

namespace MachineConnector.Config;

public class MachineConnectorConfigPath: IMachineConnectorConfigPath
{
    public string ConfigPath { get; }

    public MachineConnectorConfigPath()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            ConfigPath = @"/Users/Shared/Ofcas.MES/MachineConnector";
        else
            ConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Ofcas.MES", "MachineConnector");
    }
}