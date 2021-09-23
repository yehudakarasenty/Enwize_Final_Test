using UnityEngine;
using Newtonsoft.Json;

// NODE_3 type have two output ports
public enum GraphNodeType { ENTRY_NODE, TYPE_1, TYPE_2, TYPE_3 }

public class GraphNodeData
{
    [JsonProperty("GUID")]
    public string GUID;
    [JsonProperty("Position")]
    public Vector2 Position;
    [JsonProperty("Type")]
    public GraphNodeType Type;
    [JsonProperty("ExtraData")]
    public NodeAdditionalData AdditionalData;
}
