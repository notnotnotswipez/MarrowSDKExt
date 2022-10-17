using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using SLZ.Marrow.Warehouse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UnityEditor;
using UnityEngine;
using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;
using Microsoft.Build.Unity;
using Microsoft.CodeAnalysis;
using SLZ.MarrowEditor;
using SLZ.Marrow;
using System.Text.RegularExpressions;

namespace Maranara.Marrow
{
    public static class ElixirMixer
    {

        private static string BONELAB_DIR = null;
        public static void ExportFlasks(Pallet pallet)
        {
            List<Flask> flasks = new List<Flask>();
            foreach (Crate crate in pallet.Crates)
            {
                if (crate.GetType().IsAssignableFrom(typeof(Flask)))
                {
                    Flask flask = (Flask)crate;
                    flasks.Add(flask);
                }
            }

            string palletPath = Path.GetFullPath(ModBuilder.BuildPath);
            string flaskPath = Path.Combine(palletPath, "flasks");

            if (!Directory.Exists(flaskPath))
                Directory.CreateDirectory(flaskPath);

            foreach (Flask flask in flasks)
            {
                string title = MarrowSDK.SanitizeName(flask.Title);
                title = Regex.Replace(title, @"\s+", "");
                ExportElixirs(title, flaskPath, flask);
            }
        }

        public static void ExportElixirs(string title, string outputDirectory, Flask flask)
        {
            if (string.IsNullOrEmpty(BONELAB_DIR))
            {
                Debug.Log($"Bonelab Game Directory is null... finding directory from ModBuilder");
                foreach (var gamePath in ModBuilder.GamePathDictionary)
                {
                    if (gamePath.Value.Contains("steamapps"))
                    {
                        Debug.Log($"Found directory containing steamapps: {gamePath.Value}");
                        if (Directory.Exists(gamePath.Value))
                        {
                            Debug.Log($"Found BONELAB_DIR candidate of {gamePath.Value}");
                            BONELAB_DIR = gamePath.Value;
                            break;
                        }
                        else continue;
                    }
                }
            }
            

            List<Type> exportedTypes = new List<Type>();

            string tempDir = Path.Combine(Path.GetTempPath(), title);
            Directory.CreateDirectory(tempDir);

            // not very proud of this but hey if it works it works
            string projTemplateDir = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "BehaviourProjectTemplate");

            XDocument csproj = XDocument.Parse(File.ReadAllText(Path.Combine(projTemplateDir, "CustomMonoBehaviour.csproj")).Replace("$safeprojectname$", title).Replace("$BONELAB_DIR$", BONELAB_DIR));
            XElement compile = csproj.Root.Elements().Single((e) => e.ToString().Contains("Compile"));

            var newScriptFile = File.ReadAllLines(Path.Combine(projTemplateDir, "CustomMonoBehaviour.cs")).ToList();
            for (int i = 0; i < newScriptFile.Count; i++) newScriptFile[i] = newScriptFile[i].Replace("$safeprojectname$", title);
            int lastIndex = newScriptFile.IndexOf(newScriptFile.Single((s) => s.Contains("newshithere")));

            string[] scriptFiles = Directory.GetFiles(Application.dataPath, "*.cs", SearchOption.AllDirectories);

            FlaskLabel label = (FlaskLabel)flask.MainAsset.EditorAsset;
            foreach (Type type in label.Elixirs)
            {
                if (exportedTypes.Contains(type))
                {
                    Debug.Log("Found duplicate script, skipping");
                    continue;
                }

                Debug.Log("Searching for elixir of " + type.Name);
                string scriptPath = scriptFiles.Single((f) => Path.GetFileNameWithoutExtension(f) == type.Name);
                if (!string.IsNullOrEmpty(scriptPath))
                {
                    XElement newCompile = new XElement("Compile");
                    newCompile.SetAttributeValue("Include", Path.GetFileName(scriptPath));
                    compile.Add(newCompile);

                    SyntaxTree syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(scriptPath));
                    CompilationUnitSyntax root = syntaxTree.GetCompilationUnitRoot();
                    ClassDeclarationSyntax rootClass = null;

                    // Add the IntPtr constructor for UnhollowerRuntimeLib
                    ConstructorDeclarationSyntax ptrConstructor = SyntaxFactory.ConstructorDeclaration(type.Name).WithInitializer
                        (
                            ConstructorInitializer(SyntaxKind.BaseConstructorInitializer)
                                .AddArgumentListArguments(Argument(IdentifierName("intPtr")))
                        ).WithBody(Block());
                    ptrConstructor = ptrConstructor.AddParameterListParameters(Parameter(List<AttributeListSyntax>(), TokenList(), ParseTypeName("System.IntPtr"), Identifier("intPtr"), null));
                    ptrConstructor = ptrConstructor.AddModifiers(Token(SyntaxKind.PublicKeyword));
                    ptrConstructor = ptrConstructor.NormalizeWhitespace();
                    rootClass = MixerLibs.UpdateMainClass(root, type.Name);
                    root = root.ReplaceNode(rootClass, rootClass.AddMembers(ptrConstructor));

                    // Remove all attributes using a rewriter class
                    root = new MixerLibs.AttributeRemoverRewriter().Visit(root).SyntaxTree.GetCompilationUnitRoot();

                    // Convert the final script to a string and switch UnityAction for System.Action
                    string finalScript = root.NormalizeWhitespace().ToFullString();
                    finalScript = finalScript.Replace("[Elixir]", "");
                    finalScript = finalScript.Replace("new UnityAction", "new System.Action");
                    finalScript = finalScript.Replace("new UnityEngine.Events.UnityAction", "new System.Action");
                    // Swap StartCoroutine for MelonCoroutines.Start
                    finalScript = finalScript.Replace("this.StartCoroutine(", "MelonLoader.MelonCoroutines.Start(");
                    finalScript = finalScript.Replace("base.StartCoroutine(", "MelonLoader.MelonCoroutines.Start(");
                    finalScript = finalScript.Replace("StartCoroutine(", "MelonLoader.MelonCoroutines.Start(");

                    File.WriteAllText(Path.Combine(tempDir, Path.GetFileName(scriptPath)), finalScript);
                    //newScriptFile.Insert(lastIndex += 1, $"CustomMonoBehaviourHandler.RegisterMonoBehaviourInIl2Cpp<{type.Name}>();");

                    exportedTypes.Add(type);
                }
                else
                    Debug.LogError("FAILED TO FIND SCRIPT FOR " + type.Name + ". SKIPPING");
            }

            // xml stuff is weird so uh heres this
            string finalCsproj = csproj.ToString().Replace("xmlns=\"\" ", "");
            File.WriteAllText(Path.Combine(tempDir, "CustomMonoBehaviour.csproj"), finalCsproj);
            File.WriteAllLines(Path.Combine(tempDir, "CustomMonoBehaviour.cs"), newScriptFile);
            File.WriteAllText(Path.Combine(tempDir, "AssemblyInfo.cs"), File.ReadAllText(Path.Combine(projTemplateDir, "AssemblyInfo.cs")).Replace("$safeprojectname$", title));

            MSBuildBuildProfile profile = MSBuildBuildProfile.Create("Debug", false, "-t:Build -p:Configuration=Debug");
            List<MSBuildBuildProfile> profileList = new List<MSBuildBuildProfile>();
            profileList.Add(profile);
            IEnumerable<MSBuildBuildProfile> profiles = profileList;

            MSBuildProjectReference project = MSBuildProjectReference.FromMSBuildProject(Path.Combine(tempDir, "CustomMonoBehaviour.csproj"), profiles: profiles);

            try
            {
                project.BuildProject(profile.Name);

                if (!Directory.Exists(outputDirectory))
                    Directory.CreateDirectory(outputDirectory);

                if (File.Exists(Path.Combine(outputDirectory, $"{title}.dll")))
                    File.Delete(Path.Combine(outputDirectory, $"{title}.dll"));

                File.Copy(Path.Combine(tempDir, "bin", "Debug", title + ".dll"), Path.Combine(outputDirectory, $"{title}.dll"));
                Directory.Delete(tempDir, true);

            }
            catch (Exception e)
            {
                EditorUtility.DisplayDialog("ERROR", "Your compiled scripts had errors. Opening the output log...", "OK");
                string msbuild = $"{System.IO.Directory.GetCurrentDirectory()}\\msbuild_out.txt";
                Debug.Log(msbuild);
                System.Diagnostics.Process.Start(msbuild);
                throw e;
            }
        }
    }

}
