using UnityEngine;
using UnityEditor;
using SLZ.Marrow.Warehouse;
using Maranara.Marrow;

namespace SLZ.MarrowEditor
{
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

                bool stirred = ElixirMixer.ExportElixirs("StirTest", Application.temporaryCachePath, flask);
                if (stirred)
                    EditorUtility.DisplayDialog("Yay", "Stirred successfully with no anomalies!", "Drink the grog");
            }
            //EditorGUILayout.PropertyField(flaskInfoProperty);
        }
    }

    [CustomPreview(typeof(Flask))]
    public class FlaskPreview : CratePreview { }
}