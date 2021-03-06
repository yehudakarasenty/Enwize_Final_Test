using UnityEditor;
using System.Collections.Generic;

/// <summary>
/// Responsibility: Init Controllers
/// </summary>
[InitializeOnLoad]
public class SystemInitiator
{
    private static List<IController> controllers = new List<IController>();

    static SystemInitiator()
    {
        controllers.Add(new GraphInspectorController());
        controllers.Add(new GraphWindowController());

        foreach (IController controller in controllers)
            controller.InitDependencies();
    }
}