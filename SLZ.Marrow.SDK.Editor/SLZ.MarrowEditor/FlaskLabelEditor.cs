using NUnit.Framework;
using SLZ.Marrow.Warehouse;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

[CustomEditor(typeof(FlaskLabel))]
public class FlaskLabelEditor : Editor
{
    FlaskLabel info;

    void OnEnable()
    {
        info = target as FlaskLabel;
        toAdd = new List<Type>();
    }

    Object selectedElixir;
    Type selectedElixirType;
    List<Type> toAdd;
    private bool notAnElixir;
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

        Object newElixir = EditorGUILayout.ObjectField(selectedElixir, typeof(Object), true);
        if (selectedElixir != newElixir)
        {
            selectedElixir = newElixir;
            if (selectedElixir != null)
            {
                Type elixirType = selectedElixir.GetType();

                if (elixirType == typeof(MonoScript))
                {
                    MonoScript mono = (MonoScript)selectedElixir;
                    Type monoClass = mono.GetClass();
                    if (monoClass != null)
                        elixirType = monoClass;
                }

                Elixir attribute = (Elixir)elixirType.GetCustomAttribute(typeof(Elixir));
                if (attribute == null)
                {
                    notAnElixir = true;
                    selectedElixirType = null;
                }
                else
                {
                    notAnElixir = false;
                    selectedElixirType = elixirType;
                }
            } else
            {
                notAnElixir = false;
                selectedElixirType = null;
            }
        }

        if (notAnElixir)
        {
            EditorGUILayout.LabelField("THIS IS NOT AN ELIXIR!");
            selectedElixirType = null;
        } else if (selectedElixir != null && GUILayout.Button("Add"))
        {
            toAdd.Add(selectedElixirType);
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

    private void OnValidate()
    {
        notAnElixir = false;
        selectedElixir = null;
        selectedElixirType = null;
    }
}
