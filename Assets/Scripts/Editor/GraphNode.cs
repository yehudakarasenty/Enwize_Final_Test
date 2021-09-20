using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

// NODE_3 type have two output ports
public enum GraphNodeType { ENTRY_NODE, NODE_1, NODE_2, NODE_3 }

public class GraphNode : Node
{
    public string GUID;
    public GraphNodeType type;
}
