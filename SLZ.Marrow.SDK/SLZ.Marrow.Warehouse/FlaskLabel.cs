using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SLZ.Marrow.Warehouse
{
    public class FlaskLabel : ScriptableObject
    {
#if UNITY_EDITOR
        public MonoScript[] Elixirs {
            get {
                if (_elixirCache == null)
                {
                    List<MonoScript> types = new List<MonoScript>();
                    if (elixirNames != null)
                    {
                        foreach (string typeName in elixirNames)
                        {
                            MonoScript script = AssetDatabase.LoadAssetAtPath<MonoScript>(typeName);
                            types.Add(script);
                        }
                        _elixirCache = types.ToArray();
                    }
                    else _elixirCache = new MonoScript[0];
                }
                return _elixirCache;
            }
            set
            {
                _elixirCache = value;
                
                List<string> fullNames = new List<string>();
                foreach (MonoScript mscript in _elixirCache)
                {
                    if (mscript == null)
                        continue;

                    fullNames.Add(AssetDatabase.GetAssetPath(mscript));
                }
                elixirNames = fullNames.ToArray();
            }
        }

        private MonoScript[] _elixirCache;
#endif
        public string[] elixirNames;
        public string[] elixirPaths;
        public bool useDefaultIngredients = true;
        public string[] ingredients;
        public string[] additionalIngredients;
#if UNITY_EDITOR
        [MenuItem("Stress Level Zero/Alchemy/Create Flask Label Based on Open Scenes")]
        public static void CreateFlaskInfo()
        {
            FlaskLabel asset = ScriptableObject.CreateInstance<FlaskLabel>();
            asset.useDefaultIngredients = true;
            asset.Elixirs = Elixir.GetAllElixirsFromScene();
            AssetDatabase.CreateAsset(asset, $"Assets/Flask Label {SceneManager.GetActiveScene().name}.asset");
            AssetDatabase.SaveAssets();

            EditorUtility.FocusProjectWindow();

            Selection.activeObject = asset;
        }
#endif

    }
}
