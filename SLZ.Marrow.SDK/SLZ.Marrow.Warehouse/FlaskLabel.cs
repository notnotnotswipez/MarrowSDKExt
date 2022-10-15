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
        public Type[] Elixirs {
            get {
                if (_elixirCache == null)
                {
                    List<Type> types = new List<Type>();
                    if (elixirNames != null)
                    {
                        foreach (string typeName in elixirNames)
                        {
                            types.Add(Type.GetType(typeName));
                        }
                        _elixirCache = types.ToArray();
                    }
                    else _elixirCache = new Type[0];
                }
                return _elixirCache;
            }
            set
            {
                _elixirCache = value;
                
                List<string> fullNames = new List<string>();
                foreach (Type types in _elixirCache)
                {
                    if (types == null)
                        continue;

                    fullNames.Add(types.AssemblyQualifiedName);
                }
                elixirNames = fullNames.ToArray();
            }
        }

        private Type[] _elixirCache;
        public string[] elixirNames;

        [MenuItem("Stress Level Zero/Alchemy/Create Flask Label Based on Open Scenes")]
        public static void CreateFlaskInfo()
        {
            FlaskLabel asset = ScriptableObject.CreateInstance<FlaskLabel>();
            
            asset.Elixirs = Elixir.GetAllElixirsFromScene();
            AssetDatabase.CreateAsset(asset, "Assets/NewScripableObject.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }

        
    }
}
