using Opc.Ua;

namespace Server.Models;

public class SensorObjectState : BaseObjectState
{
    public TemperatureSensorObjectState TemperatureSensor
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

    public SensorObjectState(NodeState? parent) : base(parent)
    {
        TemperatureSensor = new TemperatureSensorObjectState(this);
    }

    protected override NodeId GetDefaultTypeDefinitionId(NamespaceTable namespaceUris)
    {
        return NodeId.Create("Sensor", MachineNodeManager.NamespaceUri, namespaceUris);
    }

    protected override void OnAfterCreate(ISystemContext context, NodeState node)
    {
        base.OnAfterCreate(context, node);

        TemperatureSensor.Create(context, null, new QualifiedName("TemperatureSensor"), null, true);
    }

    public override void GetChildren(
        ISystemContext context,
        IList<BaseInstanceState> children)
    {
        if (TemperatureSensor != null)
        {
            children.Add(TemperatureSensor);
        }

        base.GetChildren(context, children);
    }
}


