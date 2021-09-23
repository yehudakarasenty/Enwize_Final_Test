using Newtonsoft.Json;
using System.Collections.Generic;

public class GraphData
{
    [JsonProperty("nodes")]
    public List<GraphNodeData> Nodes = new List<GraphNodeData>();

    [JsonProperty("links")]
    public List<GraphNodeLinkData> Links = new List<GraphNodeLinkData>();
}
