using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(MultipleTargetGraphicsToggle), true)]
[CanEditMultipleObjects]
public class MultipleTargetGraphicsToggleEditor : ToggleEditor
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