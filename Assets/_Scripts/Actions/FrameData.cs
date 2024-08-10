using UnityEngine;

[CreateAssetMenu(menuName = "Attack Move/Frame Data")]
public class FrameData : ScriptableObject
{
    [SerializeField, Range(byte.MinValue, byte.MaxValue)] private byte _startupFrames;
    [SerializeField, Range(byte.MinValue, byte.MaxValue)] private byte _activeFrames;
    [SerializeField, Range(byte.MinValue, byte.MaxValue)] private byte _recoveryFrames;

    public byte StartupFrames => _startupFrames;
    public byte ActiveFrames => _activeFrames;
    public byte RecoveryFrames => _recoveryFrames;
}