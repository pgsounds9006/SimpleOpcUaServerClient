using Opc.Ua;

namespace Server.Models;

public class TemperatureSensorObjectState : BaseObjectState
{
    public TemperatureSensorValueVariableState Temperature
    {
        get;
        set
        {
            if (!ReferenceEquals(field, value))
            {
                ChangeMasks |= NodeStateChangeMasks.Children;
            }

            field = value;
        }
    }

    public TemperatureSensorObjectState(NodeState? parent) : base(parent)
    {
        Temperature = new TemperatureSensorValueVariableState(this);
    }

    protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
    {
        return NodeId.Create("Sensor_Temperature", MachineNodeManager.NamespaceUri, namespaceUris);
    }

    protected override void OnAfterCreate(ISystemContext context, NodeState node)
    {
        base.OnAfterCreate(context, node);

        Temperature.Create(context, null, new QualifiedName("Temperature"), null, true);
    }

    public override void GetChildren(
        ISystemContext context,
        IList<BaseInstanceState> children)
    {
        if (Temperature != null)
        {
            children.Add(Temperature);
        }

        base.GetChildren(context, children);
    }
}


