using UnityEngine;
using UnityEditor;
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
        EditorApplication.update -= RunOnce;
        string projectPath = Application.dataPath + "\\..";
        string behaviourProject = Path.Combine(projectPath, "BehaviourProjectTemplate");
        if (!Directory.Exists(behaviourProject))
        {
            Debug.Log("BehaviourProjectTemplate does not exist. Copying to project folder...");
            string dependencyPath = Path.Combine(projectPath, "Packages\\com.stresslevelzero.marrow.sdk\\DependencyBriefcase");
            string behaviourProjectTemplate = Path.Combine(dependencyPath, "BehaviourProjectTemplate");
            Directory.CreateDirectory(behaviourProject);
            CopyFilesRecursively(behaviourProjectTemplate, behaviourProject);
        }
    }

    private static void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        //Now Create all of the directories
        foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
        {
            Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
        }

        //Copy all the files & Replaces any files with the same name
        foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
        {
            string fileName = Path.GetFileName(newPath);
            string finalPath = newPath.Replace(sourcePath, targetPath);
            if (fileName.EndsWith(".csprog"))
            {
                finalPath = finalPath.Replace(".csprog", ".csproj");
            }
            File.Copy(newPath, finalPath, true);
        }
    }
}