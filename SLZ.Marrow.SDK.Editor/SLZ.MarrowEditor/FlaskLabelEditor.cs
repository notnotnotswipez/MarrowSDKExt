using NUnit.Framework;
using SLZ.Marrow.Warehouse;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FlaskLabel))]
public class FlaskLabelEditor : Editor
{
    FlaskLabel info;

    void OnEnable()
    {
        info = target as FlaskLabel;
        toAdd = new List<Type>();
    }

    MonoBehaviour selectedMB;
    List<Type> toAdd;
    public override void OnInspectorGUI()
    {
        
        serializedObject.Update();

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

        Type toRemove = null;
        foreach (Type type in info.Elixirs)
        {
            if (type == null)
                continue;
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(type.FullName);
            if (GUILayout.Button("X"))
            {
                toRemove = type;
                break;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.LabelField($"There are ({info.Elixirs.Length}) Elixirs total.");
        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField($"Add an Elixir", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();

        selectedMB = (MonoBehaviour)EditorGUILayout.ObjectField(selectedMB, typeof(MonoBehaviour), true);
        
        if (selectedMB != null)
        {
            Elixir attribute = (Elixir)selectedMB.GetType().GetCustomAttribute(typeof(Elixir));
            if (attribute == null)
            {
                EditorGUILayout.LabelField("THIS IS NOT AN ELIXIR!");
            }
            else if (GUILayout.Button("Add"))
            {
                toAdd.Add(selectedMB.GetType());
                selectedMB = null;
            }
        }
        EditorGUILayout.EndHorizontal();

        if (GUILayout.Button("Add All from Current Scene"))
        {
            toAdd.AddRange(Elixir.GetAllElixirsFromScene());
            selectedMB = null;
        }

        if (toRemove != null || toAdd.Count != 0)
        {
            List<Type> types = new List<Type>();
            types.AddRange(info.Elixirs);
            if (toRemove != null)
                types.Remove(toRemove);

            if (toAdd.Count != 0)
            {
                foreach (Type type in toAdd)
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

        serializedObject.ApplyModifiedProperties();
    }
}
