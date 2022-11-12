using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
public class Elixir : Attribute
{
#if UNITY_EDITOR
    public static MonoScript[] GetAllElixirsFromScene()
    {
        List<MonoScript> elixirs = new List<MonoScript>();
        MonoScript[] mbs = Resources.FindObjectsOfTypeAll<MonoScript>();
        foreach (MonoScript mb in mbs)
        {
            Type type = mb.GetType();
            Elixir attribute = (Elixir)type.GetCustomAttribute(typeof(Elixir));
            if (attribute == null)
                continue;
            else elixirs.Add(mb);

            break;
        }
        return elixirs.ToArray();
    }
#endif
}

[AttributeUsage(AttributeTargets.Class)]
public class DontAssignIntPtr : Attribute
{
    
}