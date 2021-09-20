using UnityEngine;
using Newtonsoft.Json;

public class GraphNodeLinkData
{
    [JsonProperty("BaseNodeGUID")]
    public string BaseNodeGuid;
    [JsonProperty("PortName")]
    public string PortName;
    [JsonProperty("TargetNodeGuid")]
    public string TargetNodeGuid;
}
