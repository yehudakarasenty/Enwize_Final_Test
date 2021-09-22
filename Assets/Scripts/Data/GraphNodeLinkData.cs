using Newtonsoft.Json;

public class GraphNodeLinkData
{
    [JsonProperty("BaseNodeGUID")]
    public string BaseNodeGuid;
    [JsonProperty("TargetNodeGuid")]
    public string TargetNodeGuid;
    [JsonProperty("portName")]
    public string portName;
}
