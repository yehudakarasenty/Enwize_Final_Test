using UnityEditor;
using System.Collections.Generic;

[InitializeOnLoad]
public class SystemInitiator
{
    private static List<IController> controllers = new List<IController>();
    static SystemInitiator()
    {
        controllers.Add(new GraphInspectorController());
        controllers.Add(new GraphWIndowController());

        foreach (IController controller in controllers)
            controller.InitDependencies();
    }
}