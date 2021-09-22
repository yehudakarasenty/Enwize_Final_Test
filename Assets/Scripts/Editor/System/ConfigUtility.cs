using UnityEditor;
using UnityEngine;

/// <summary>
/// Responsibility: Load and provide config file
/// </summary>
[InitializeOnLoad]
public class ConfigUtility
{
    public static Configuration Configuration { get; private set; }

    static ConfigUtility() => Configuration = Resources.Load<Configuration>("Config");
}