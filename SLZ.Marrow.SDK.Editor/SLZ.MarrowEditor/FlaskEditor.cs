using UnityEngine;
using UnityEditor;
using SLZ.Marrow.Warehouse;
using Maranara.Marrow;
using UnityEngine.Events;

namespace SLZ.MarrowEditor
{
#if UNITY_EDITOR
    [CustomEditor(typeof(Flask))]
    [CanEditMultipleObjects]
    public class FlaskEditor : CrateEditor
    {
        SerializedProperty flaskInfoProperty;

        public override void OnEnable()
        {
            base.OnEnable();
            flaskInfoProperty = serializedObject.FindProperty("_mainAsset");
        }
        public override void OnInspectorGUIBody()
        {
            base.OnInspectorGUIBody();
            if (GUILayout.Button("Stir Flask"))
            {
                Flask flask = (Flask)target;

                UnityEvent<bool> BuildEvent = new UnityEvent<bool>();
                BuildEvent.AddListener(OnBuildComplete);

                ElixirMixer.ExportElixirs("StirTest", Application.temporaryCachePath, flask, BuildEvent);
                /*if (stirred)
                   */
            }
            //EditorGUILayout.PropertyField(flaskInfoProperty);
        }

        private static void OnBuildComplete(bool hasErrors)
        {
            if (hasErrors)
            {
                EditorUtility.DisplayDialog("Error", $"Errors detected in the Flask! Check the Console for errors.", "Fine");
            }
            else EditorUtility.DisplayDialog("Yay", "Stirred successfully with no anomalies!", "Drink the grog");
        }
    }

    [CustomPreview(typeof(Flask))]
    public class FlaskPreview : CratePreview { }
#endif
}