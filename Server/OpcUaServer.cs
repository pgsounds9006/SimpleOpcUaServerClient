using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Opc.Ua;
using Opc.Ua.Server;

namespace Server;

public class OpcUaServer : StandardServer
{
    protected override MasterNodeManager CreateMasterNodeManager(IServerInternal server, ApplicationConfiguration configuration)
    {
        Utils.Trace("Creating the Node Managers.");
        return new MasterNodeManager(server, configuration, null, [new MachineNodeManager(server, configuration)]);
    }

    /// <summary>
    /// Loads the non-configurable properties for the application.
    /// </summary>
    /// <remarks>
    /// These properties are exposed by the server but cannot be changed by administrators.
    /// </remarks>
    protected override ServerProperties LoadServerProperties()
    {
        var properties = new ServerProperties
        {
            ManufacturerName = "pgsounds9006",
            ProductName = "pgsounds9006 InformationModel Server",
            ProductUri = "https://pgsounds9006.co.kr/OpcUa/InformationModelServer/v1.0",
            SoftwareVersion = Utils.GetAssemblySoftwareVersion(),
            BuildNumber = Utils.GetAssemblyBuildNumber(),
            BuildDate = Utils.GetAssemblyTimestamp()
        };

        // TBD - All applications have software certificates that need to added to the properties.

        return properties;
    }
}
