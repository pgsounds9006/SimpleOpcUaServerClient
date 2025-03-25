using Opc.Ua;
using Opc.Ua.Server;
using Server.Models;

namespace Server;

public class MachineNodeManager : CustomNodeManager2
{
    public const string NamespaceUri = "https://pgsounds9006.co.kr//Machine";
    readonly SensorObjectState sensor1;
    readonly SensorObjectState sensor2;
    Timer? valueGetterTimer;

    uint nodeIdCounter;

    public MachineNodeManager(IServerInternal server, ApplicationConfiguration configuration)
        : base(server, configuration)
    {
        SystemContext.NodeIdFactory = this;
        SetNamespaces(NamespaceUri);
        sensor1 = new SensorObjectState(null);
        sensor2 = new SensorObjectState(null);
    }


    public override NodeId New(ISystemContext context, NodeState node)
    {
        return new NodeId(++nodeIdCounter, NamespaceIndex);
    }

    public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
    {
        lock (Lock)
        {
            LoadPredefinedNodes(SystemContext, externalReferences);

            // initialize it from the type model and assign unique node ids.
            sensor1.Create(
                SystemContext,
                null,
                new QualifiedName("Sensor #1", NamespaceIndex),
                null,
                true);

            sensor2.Create(
                SystemContext,
                null,
                new QualifiedName("Sensor #2", NamespaceIndex),
                null,
                true);

            // link root to objects folder.
            IList<IReference>? references = null;

            if (!externalReferences.TryGetValue(ObjectIds.ObjectsFolder, out references))
            {
                externalReferences[ObjectIds.ObjectsFolder] = references = [];
            }

            references.Add(new NodeStateReference(ReferenceTypeIds.Organizes, false, sensor1.NodeId));
            references.Add(new NodeStateReference(ReferenceTypeIds.Organizes, false, sensor2.NodeId));

            // store it and all of its children in the pre-defined nodes dictionary for easy look up.
            AddPredefinedNode(SystemContext, sensor1);
            AddPredefinedNode(SystemContext, sensor2);

            valueGetterTimer = new Timer(SetVariableValue, null, 0, 10);
        }
    }

    private void SetVariableValue(object? state)
    {
        try
        {
            sensor1.TemperatureSensor.Temperature.Value = Random.Shared.Next(1, 100);
            sensor1.ClearChangeMasks(SystemContext, true);

            sensor2.TemperatureSensor.Temperature.Value = Random.Shared.Next(1, 100);
            sensor2.ClearChangeMasks(SystemContext, true);
        }
        catch (Exception e)
        {
            Utils.Trace(e, "Unexpected error during simulation.");
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (valueGetterTimer != null)
            {
                Utils.SilentDispose(valueGetterTimer);
                valueGetterTimer = null;
            }
        }
    }
}
