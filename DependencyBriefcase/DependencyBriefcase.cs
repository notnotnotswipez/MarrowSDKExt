using UnityEngine;
using UnityEditor;
using System.Diagnostics;
using System.IO;

[InitializeOnLoad]
public class DependencyBriefcase
{
    static DependencyBriefcase()
    {
        EditorApplication.update += RunOnce;
    }

    static void RunOnce()
    {
        Debug.Log("Checking BehaviourProjectTemplate");
        string behaviourProject = Path.Combine(Application.dataPath, "BehaviourProjectTemplate");
        if (!File.Exists(behaviourProject))
        {
            Debug.Log("BehaviourProjectTemplate does not exist. Copying to project folder...")
            string dependencyPath = Path.Combine(Application.dataPath, "Packages\\com.stresslevelzero.marrow.sdk\\DependencyBriefcase");
            string behaviourProjectTemplate = Path.Combine(dependencyPath, "BehaviourProjectTemplate");
            File.Copy(behaviourProjectTemplate, Application.dataPath, true);
        }
        
    }
}