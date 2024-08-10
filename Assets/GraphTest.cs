using UnityEngine;

public class GraphTest : MonoBehaviour
{
    [SerializeField] private AnimationCurve _curve;

    public void SelfUpdate(Vector3 position)
    {
        Keyframe keyframe = new Keyframe(Time.time, position.x, 0, 0, 0, 0);
        _curve.AddKey(keyframe);
    }
}