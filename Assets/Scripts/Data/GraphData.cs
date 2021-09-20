using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class GraphData : MonoBehaviour
{
    [JsonProperty("nodes")]
    List<GraphNodeData> nodes = new List<GraphNodeData>();
    [JsonProperty("links")]
    List<GraphNodeLinkData> links = new List<GraphNodeLinkData>();
}
