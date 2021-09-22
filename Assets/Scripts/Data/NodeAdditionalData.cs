using Newtonsoft.Json;
public class NodeAdditionalData
{
    [JsonProperty("mySpecialNumber")]
    public int mySpecialNumber;
    [JsonProperty("mySpecialSecret ")]
    public string mySpecialSecret;

    public NodeAdditionalData()
    {
    }

    public NodeAdditionalData(NodeAdditionalData copy)
    {
        mySpecialNumber = copy.mySpecialNumber;
        mySpecialSecret = copy.mySpecialSecret;
    }

    public NodeAdditionalData(int mySpecialNumber, string mySpecialSecret)
    {
        this.mySpecialNumber = mySpecialNumber;
        this.mySpecialSecret = mySpecialSecret;
    }
}