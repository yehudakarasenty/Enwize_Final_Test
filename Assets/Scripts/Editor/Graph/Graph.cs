using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Responsebility: View of the graph; Create and remove nodes
/// </summary>
public class Graph : GraphView
{
    private readonly Vector2 nodeSize = new Vector2(150, 200);

    public Graph()
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

        //Create entry node
        CreateNode(GraphNodeType.ENTRY_NODE, new Vector2(100, 200));
    }

    private Port GeneratePort(GraphNode node, Direction portDiraction, Port.Capacity capacity = Port.Capacity.Single)
    {
        return node.InstantiatePort(Orientation.Horizontal, portDiraction, capacity, typeof(float)); // Arbitrary type
    }

    public GraphNode CreateNode(GraphNodeType nodeType, Vector2 position, string guid = "")
    {
        GraphNode node = new GraphNode
        {
            title = nodeType.ToString(), //TODO need to be automatic
            GUID = string.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString() : guid,
            type = nodeType
        };

        if (nodeType != GraphNodeType.ENTRY_NODE)
        {
            Port inputPort = GeneratePort(node, Direction.Input, Port.Capacity.Multi);
            inputPort.portName = "Input";
            node.inputContainer.Add(inputPort);
        }

        Port outputPort = GeneratePort(node, Direction.Output);
        outputPort.portName = "Output-0";
        node.outputContainer.Add(outputPort);

        if(nodeType == GraphNodeType.NODE_3)
        {
            Port secondOutputPort = GeneratePort(node, Direction.Output);
            secondOutputPort.portName = "Output-1";
            node.outputContainer.Add(secondOutputPort);
        }

        node.RefreshExpandedState();
        node.RefreshPorts();
        node.SetPosition(new Rect(position, nodeSize));

        AddElement(node);

        return node;
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        List<Port> compatiblePorts = new List<Port>();
        ports.ForEach((port) =>
        {
            if (startPort != port && startPort.node != port.node)
            {
                //Exclusive node connection option //TODO by config
                switch (((GraphNode)startPort.node).type)
                {
                    case GraphNodeType.ENTRY_NODE:
                        compatiblePorts.Add(port);
                        break;
                    case GraphNodeType.NODE_1:
                        if(((GraphNode)port.node).type == GraphNodeType.NODE_2)
                            compatiblePorts.Add(port);
                        break;
                    case GraphNodeType.NODE_2:
                        if (((GraphNode)port.node).type == GraphNodeType.NODE_3)
                            compatiblePorts.Add(port);
                        break;
                    case GraphNodeType.NODE_3:
                        if (((GraphNode)port.node).type == GraphNodeType.NODE_1)
                            compatiblePorts.Add(port);
                        break;
                    default:
                        break;
                }
            }
        });

        return compatiblePorts;
    }

    public void ClearGraph()
    {
        foreach (GraphNode node in nodes.ToList().Cast<GraphNode>().ToList())
        {
            edges.ToList().Where(x => x.input.node == node).ToList()
                .ForEach(edge => RemoveElement(edge));
            RemoveElement(node);
        }
    }
}
