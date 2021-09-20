using UnityEngine;
using Newtonsoft.Json;

// NODE_3 type have two output ports
public enum GraphNodeType { ENTRY_NODE, NODE_1, NODE_2, NODE_3 }

public class GraphNodeData
{
    [JsonProperty("GUID")]
    public string GUID;
    [JsonProperty("Position")]
    public Vector2 Position;
    [JsonProperty("Type")]
    public GraphNodeType type;
}
