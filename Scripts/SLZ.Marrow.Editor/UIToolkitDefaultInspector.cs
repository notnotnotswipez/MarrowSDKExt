 
 
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CanEditMultipleObjects]
[CustomEditor(typeof(UnityEngine.Object), editorForChildClasses: true)]
public class UIToolkitDefaultInspector : Editor
{
    public override VisualElement CreateInspectorGUI()
    {
        var root = new VisualElement();
        UnityEditor.UIElements.InspectorElement.FillDefaultInspector(root, serializedObject, this);
        return root;
    }
}