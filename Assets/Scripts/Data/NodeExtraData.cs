using Newtonsoft.Json;
public class NodeExtraData
{
    [JsonProperty("mySpecialNumber")]
    public int mySpecialNumber;
    [JsonProperty("mySpecialSecret ")]
    public string mySpecialSecret;

    public NodeExtraData()
    {
    }

    public NodeExtraData(NodeExtraData copy)
    {
        mySpecialNumber = copy.mySpecialNumber;
        mySpecialSecret = copy.mySpecialSecret;
    }

    public NodeExtraData(int mySpecialNumber, string mySpecialSecret)
    {
        this.mySpecialNumber = mySpecialNumber;
        this.mySpecialSecret = mySpecialSecret;
    }
}