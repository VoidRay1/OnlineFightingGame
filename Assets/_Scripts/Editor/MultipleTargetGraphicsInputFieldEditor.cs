using TMPro.EditorUtilities;
using UnityEditor;

[CustomEditor(typeof(MultipleTargetGraphicsInputField), true)]
[CanEditMultipleObjects]
public class MultipleTargetGraphicsInputFieldEditor : TMP_InputFieldEditor
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