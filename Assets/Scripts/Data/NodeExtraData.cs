using Newtonsoft.Json;
public class NodeExtraData
{
    [JsonProperty("mySpecialNumber")]
    public int mySpecialNumber;
    [JsonProperty("mySpecialSecret ")]
    public string mySpecialSecret;
}