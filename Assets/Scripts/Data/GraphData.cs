using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class GraphData
{
    [JsonProperty("nodes")]
    public List<GraphNodeData> nodes = new List<GraphNodeData>();
    [JsonProperty("links")]
    public List<GraphNodeLinkData> links = new List<GraphNodeLinkData>();
}
