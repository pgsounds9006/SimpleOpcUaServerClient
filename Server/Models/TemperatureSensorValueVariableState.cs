using Opc.Ua;

namespace Server.Models;

public class TemperatureSensorValueVariableState(NodeState parent) : AnalogItemState<double>(parent)
{
    protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
    {
        return NodeId.Create("Sensor_Temperature_Value", MachineNodeManager.NamespaceUri, namespaceUris);
    }
}
