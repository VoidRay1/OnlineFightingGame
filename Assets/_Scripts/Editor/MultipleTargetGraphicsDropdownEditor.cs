using UnityEditor;
using TMPro.EditorUtilities;

[CustomEditor(typeof(MultipleTargetGraphicsDropdown), true)]
[CanEditMultipleObjects]
public class MultipleTargetGraphicsDropdownEditor : DropdownEditor
{
    private SerializedProperty _targetGraphics;

    protected override void OnEnable()
    {
        base.OnEnable();
        _targetGraphics = serializedObject.FindProperty(nameof(_targetGraphics));
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        EditorGUILayout.PropertyField(_targetGraphics);
        serializedObject.ApplyModifiedProperties();
    }
}