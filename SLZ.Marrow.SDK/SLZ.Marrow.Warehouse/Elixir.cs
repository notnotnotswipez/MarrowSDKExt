using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class Elixir : Attribute
{
    public static Type[] GetAllElixirsFromScene()
    {
        List<Type> elixirs = new List<Type>();
        MonoBehaviour[] mbs = Resources.FindObjectsOfTypeAll<MonoBehaviour>();
        foreach (MonoBehaviour mb in mbs)
        {
            Type type = mb.GetType();
            Elixir attribute = (Elixir)type.GetCustomAttribute(typeof(Elixir));
            if (attribute == null)
                continue;
            else elixirs.Add(type);

            break;
        }
        return elixirs.ToArray();
    }

}