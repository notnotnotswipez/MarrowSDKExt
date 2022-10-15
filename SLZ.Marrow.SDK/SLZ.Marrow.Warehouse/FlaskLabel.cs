using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SLZ.Marrow.Warehouse
{
    public class FlaskLabel : ScriptableObject
    {
        public Type[] elixirs;

        [MenuItem("Stress Level Zero/Alchemy/Create Flask Label Based on Open Scenes")]
        public static void CreateFlaskInfo()
        {
            FlaskLabel info = CreateInstance<FlaskLabel>();
            info.elixirs = Elixir.GetAllElixirsFromScene();
            AssetDatabase.CreateAsset(info, "Assets/FlaskInfo.asset");
            EditorUtility.SetDirty(info);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        
    }
}
