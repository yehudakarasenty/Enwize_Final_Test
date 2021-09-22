using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class ConfigUtility 
{
    public static Configuration Configuration { get; private set; }

    static ConfigUtility()
    {
        Configuration = Resources.Load<Configuration>("Config");
    }
}