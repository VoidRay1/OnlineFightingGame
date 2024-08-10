using UnityEngine;

public class HitBox : MonoBehaviour
{
    [SerializeField] private BoxCollider _boxCollider;
    private AttackMoveData _moveData;

    private void Awake()
    {
        Disable();
    }

    public void Initialize(AttackMoveData moveData, Transform parent)
    {
        _moveData = moveData;
        transform.parent = parent;
        transform.SetLocalPositionAndRotation(moveData.HitBoxData.Position, moveData.HitBoxData.Rotation);
        _boxCollider.size = moveData.HitBoxData.ColliderSize;
    }

    public void Enable()
    {
        enabled = true;
    }

    public void Disable()
    {
        enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (enabled)
        {
            if (other.TryGetComponent(out HurtBox hurtBox))
            {
                hurtBox.TakeDamage(_moveData.MoveProperties.Damage);
                if (other.transform.parent.TryGetComponent(out ChampionStateMachine championStateMachine))
                {
                    if (_moveData.AttackType == AttackType.Sweep)
                    {
                        championStateMachine.SwitchStateInstantly<ChampionSweepFallState>();
                    }
                    if (_moveData.AttackType == AttackType.Punch)
                    {
                        championStateMachine.SwitchStateInstantly<ChampionReceivePunchState>();
                    }
                    if (_moveData.AttackType == AttackType.Uppercut)
                    {
                        championStateMachine.SwitchStateInstantly<ChampionReceiveUppercutState>();
                    }
                }
            }
        }
    }
}