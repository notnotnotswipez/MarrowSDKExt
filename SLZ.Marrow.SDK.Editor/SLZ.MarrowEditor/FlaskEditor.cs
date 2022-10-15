using UnityEngine;
using UnityEditor;
using SLZ.Marrow.Warehouse;

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
            //EditorGUILayout.PropertyField(flaskInfoProperty);
        }
    }

    [CustomPreview(typeof(Flask))]
    public class FlaskPreview : CratePreview { }
}