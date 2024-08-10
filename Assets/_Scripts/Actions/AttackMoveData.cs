using UnityEngine;

[CreateAssetMenu(menuName = "Attack Move/Move Data")]
public class AttackMoveData : ScriptableObject
{
    [SerializeField] private string _name;
    [SerializeField] private MoveProperties _moveProperties;
    [SerializeField] private FrameData _frameData;
    [SerializeField] private HitBoxData _hitBoxData;
    [SerializeField] private AttackType _attackType;

    public string Name => _name;
    public MoveProperties MoveProperties => _moveProperties;
    public FrameData FrameData => _frameData;
    public HitBoxData HitBoxData => _hitBoxData;
    public AttackType AttackType => _attackType;
}