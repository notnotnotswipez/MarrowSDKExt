using Maranara.Marrow;
using NUnit.Framework;
using SLZ.Marrow.Warehouse;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static Codice.CM.WorkspaceServer.WorkspaceTreeDataStore;
using Object = UnityEngine.Object;

[CustomEditor(typeof(FlaskLabel))]
public class FlaskLabelEditor : Editor
{
    FlaskLabel info;

    void OnEnable()
    {
        info = target as FlaskLabel;
        toAdd = new List<MonoScript>();
        ingredientsProperty = serializedObject.FindProperty("ingredients");
        ingredientsPlusProperty = serializedObject.FindProperty("additionalIngredients");
    }

    private MonoScript selectedElixir;
    private List<MonoScript> toAdd;
    private MonoScript toRemove;
    private bool notAnElixir, ingredientsInfoFoldout, elixirInfoFoldout, elixirListFoldout;

    private SerializedProperty ingredientsProperty;
    private SerializedProperty ingredientsPlusProperty;
    public override void OnInspectorGUI()
    {
        
        serializedObject.Update();

        #region ElixirSelector
        EditorGUILayout.LabelField("Elixirs", EditorStyles.boldLabel);
        if (info == null)
        {
            EditorGUILayout.LabelField("Info is null");
            return;
        }
        if (info.Elixirs == null)
        {
            EditorGUILayout.LabelField("Elixir is null");
            return;
        }

        elixirInfoFoldout = EditorGUILayout.Foldout(elixirInfoFoldout, "Usage Info", true);
        if (elixirInfoFoldout)
        {
            EditorStyles.label.wordWrap = true;
            EditorGUILayout.LabelField("Elixirs are Mono Scripts that will be compiled into your Flask. This means MonoBehaviours and ScriptableObjects are supported, with nested types such as structs being \"supported.\" Supported in the fact that they will compile as a part of their parent Elixir, but will not show up in any Elixir list.");
        }
        EditorGUILayout.Space(5);

        toRemove = null;

        GUIStyle style = EditorStyles.foldout;

        style.fontStyle = FontStyle.Bold;
        elixirListFoldout = EditorGUILayout.Foldout(elixirListFoldout, "Elixir List", true, style);
        style.fontStyle = FontStyle.Normal;

        if (elixirListFoldout)
        {
            for (int i = 0; i < info.Elixirs.Length; i++)
            {
                MonoScript type = info.Elixirs[i];
                if (type == null)
                    continue;
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(info.elixirNames[i]);
                if (GUILayout.Button("X"))
                {
                    toRemove = type;
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
        }
        

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField($"There are ({info.Elixirs.Length}) Elixirs total.");
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField($"Add an Elixir");

        MonoScript newElixir = (MonoScript)EditorGUILayout.ObjectField(selectedElixir, typeof(MonoScript), true);
        if (selectedElixir != newElixir)
        {
            selectedElixir = newElixir;
            if (selectedElixir != null)
            {
                Type elixirType = selectedElixir.GetClass();

                Elixir attribute = (Elixir)elixirType.GetCustomAttribute(typeof(Elixir));
                if (attribute == null)
                {
                    notAnElixir = true;
                }
                else
                {
                    notAnElixir = false;
                }
            } else
            {
                notAnElixir = false;
            }
        }

        if (notAnElixir)
        {
            EditorGUILayout.LabelField("THIS IS NOT AN ELIXIR!");
        } else if (selectedElixir != null && GUILayout.Button("Add"))
        {
            toAdd.Add(selectedElixir);
            selectedElixir = null;
        }

        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Add All from Current Scene"))
        {
            toAdd.AddRange(Elixir.GetAllElixirsFromScene());
            selectedElixir = null;
        }

        if (toRemove != null || toAdd.Count != 0)
        {
            List<MonoScript> types = new List<MonoScript>();
            types.AddRange(info.Elixirs);
            if (toRemove != null)
                types.Remove(toRemove);

            if (toAdd.Count != 0)
            {
                foreach (MonoScript type in toAdd)
                {
                    if (!types.Contains(type))
                        types.Add(type);
                }
                toAdd.Clear();
            }

            info.Elixirs = types.ToArray();

            EditorUtility.SetDirty(info);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        #endregion

        GUILayout.Space(20);
        #region ReferenceSelector
        EditorGUILayout.LabelField("Ingredients", EditorStyles.boldLabel);

        ingredientsInfoFoldout = EditorGUILayout.Foldout(ingredientsInfoFoldout, "Usage Info", true);
        if (ingredientsInfoFoldout)
        {
            EditorStyles.label.wordWrap = true;
            EditorGUILayout.LabelField("Ingredients are references to other assemblies that your Flask will use. It is recommended you keep these set to default, as the compiler will omit any references to Ingredients not used in the Flask. The Ingredients are relative to the MelonLoader/Managed folder, so if you want to reference something such as another Flask, you'll need to enter the path relative to the MelonLoader/Managed folder. While you can enter the full path, it is recommended you keep the path relative in the event that either your MelonLoader directory moves, or you're working with other people who have their directory in a different location.");
        }
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Use Default Ingredients");
        bool defaultIngredients = EditorGUILayout.Toggle(info.useDefaultIngredients);
        EditorGUILayout.EndHorizontal();

        if (defaultIngredients != info.useDefaultIngredients)
        {
            info.useDefaultIngredients = defaultIngredients;
            if (defaultIngredients)
            {
                if (info.ingredients == null)
                {
                    info.ingredients = ElixirMixer.GetDefaultReferences(false);
                }
            }
        }
        if (!defaultIngredients)
        {
            GUIContent content = new GUIContent()
            {
                text = "Base Ingredients"
            };
            EditorGUILayout.PropertyField(ingredientsProperty, content);

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Base Ingredients to Default"))
            {
                info.ingredients = ElixirMixer.GetDefaultReferences(false);
            }
            if (GUILayout.Button("Clear Base Ingredients"))
            {
                info.ingredients = new string[0];
            }
            if (GUILayout.Button("Select Base Ingredient"))
            {
                SelectIngredient(ref info.ingredients);
            }
            EditorGUILayout.EndHorizontal();

            
        }
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(ingredientsPlusProperty);
        if (GUILayout.Button("Select Additional Ingredient"))
        {
            SelectIngredient(ref info.additionalIngredients);
        }
        #endregion

        serializedObject.ApplyModifiedProperties();
    }

    private static void SelectIngredient(ref string[] arr)
    {
        string path = GetIngredient();
        if (!string.IsNullOrEmpty(path))
        {
            List<string> gredients = new List<string>();
            gredients.AddRange(arr);
            if (!gredients.Contains(path))
                gredients.Add(path);
            arr = gredients.ToArray();
        }
    }

    private static string GetIngredient()
    {
        if (!ElixirMixer.ConfirmMelonDirectory())
            return string.Empty;

        string GotPath = EditorUtility.OpenFilePanel("Select an Ingredient", ElixirMixer.ML_DIR, "dll");
        if (string.IsNullOrEmpty(GotPath))
            return string.Empty;

        return Path.GetRelativePath(ElixirMixer.ML_MANAGED_DIR, GotPath);
    }

    private void OnValidate()
    {
        notAnElixir = false;
        selectedElixir = null;
    }
}
