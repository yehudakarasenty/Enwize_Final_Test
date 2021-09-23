using Newtonsoft.Json;
public class NodeAdditionalData
{
    [JsonProperty("mySpecialNumber")]
    public int MySpecialNumber;
    [JsonProperty("mySpecialSecret ")]
    public string MySpecialSecret;

    public NodeAdditionalData(){}

    public NodeAdditionalData(NodeAdditionalData copy)
    {
        MySpecialNumber = copy.MySpecialNumber;
        MySpecialSecret = copy.MySpecialSecret;
    }

    public NodeAdditionalData(int mySpecialNumber, string mySpecialSecret)
    {
        MySpecialNumber = mySpecialNumber;
        MySpecialSecret = mySpecialSecret;
    }
}