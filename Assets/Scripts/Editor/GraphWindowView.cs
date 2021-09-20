using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class GraphWindowView : GraphView
{
    private readonly Vector2 nodeSize = new Vector2(150, 200);

    public GraphWindowView()
    {
        //Add style
        styleSheets.Add(Resources.Load<StyleSheet>("Graph"));

        //Add the ability to set zoom
        SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

        //Add selection and drag ability
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        //Add grid background
        var gridBackground = new GridBackground();
        Insert(0, gridBackground);
        gridBackground.StretchToParentSize();

        CreateEntryNode();
    }

    private Port GeneratePort(GraphNode node, Direction portDiraction, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDiraction, capacity, typeof(float)); // Arbitrary type
    }

    public GraphNode CreateNode(string nodeName, GraphNodeType nodeType)
    {
        GraphNode node = new GraphNode
        {
            title = nodeName,
            GUID = Guid.NewGuid().ToString(),
            type = nodeType
        };

        Port inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
        inputPort.portName = "Input";
        node.inputContainer.Add(inputPort);

        Port outputPort = GeneratePort(node, Direction.Output);
        outputPort.portName = "Next"; //TO Understand: different from him, becuse he named it by choice
        node.outputContainer.Add(outputPort);

        if(nodeType == GraphNodeType.NODE_3)
        {
            Port secondOutputPort = GeneratePort(node, Direction.Output);
            secondOutputPort.portName = "Next"; //TO Understand: different from him, becuse he named it by choice
            node.outputContainer.Add(secondOutputPort);
        }

        node.RefreshExpandedState();
        node.RefreshPorts();
        node.SetPosition(new Rect(Vector2.zero, nodeSize));

        AddElement(node);

        return node;
    }

    private GraphNode CreateEntryNode()
    {
        GraphNode node = new GraphNode
        {
            title = "Start",
            GUID = Guid.NewGuid().ToString(),
            type = GraphNodeType.ENTRY_NODE
        };

        Port port = GeneratePort(node, Direction.Output);
        port.portName = "Next"; //To understand. for saving?
        node.outputContainer.Add(port);

        node.RefreshExpandedState();
        node.RefreshPorts();

        node.SetPosition(new Rect(new Vector2(100,200), nodeSize));
        AddElement(node);

        return node;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node) //TODO maybe here need to check Exclusive node connection option 
                compatiblePorts.Add(port);
        });

        return compatiblePorts;
    }
}
