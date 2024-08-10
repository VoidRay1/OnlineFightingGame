using UnityEditor;
using UnityEditor.UI;

[CustomEditor(typeof(MultipleTargetGraphicsSlider), true)]
[CanEditMultipleObjects]
public class MultipleTargetGraphicsSliderEditor : SliderEditor
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
