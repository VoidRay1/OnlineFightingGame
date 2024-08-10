using UnityEngine;

public abstract class ChampionAttackState : ChampionBaseState
{
    private readonly AttackType _attackType;
    private bool _isHitBoxTimerStarted;

    protected readonly HitBox HitBox;
    protected readonly Transform HitBoxParent;
    protected AttackMoveData MoveData;

    public bool IsHitBoxTimerStarted => _isHitBoxTimerStarted;
    public AttackType AttackType => _attackType;

    protected ChampionAttackState(Animator animator, IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions,
        Champion champion, Transform hitBoxParent, AttackType attackType) 
        : base(animator, championStateSwitcher, moveListActions, champion)
    {
        HitBox = champion.HitBox;
        HitBoxParent = hitBoxParent;
        _attackType = attackType;
    }

    public override void Enter()
    {
        HitBox.Initialize(MoveData, HitBoxParent);
    }

    public void SetMoveData(AttackMoveData moveData)
    {
        MoveData = moveData;
    }

/*    public void StartHitBoxTimer()
    {
        _isHitBoxTimerStarted = true;
        float timeToEnableHitBox = MoveData.HitBoxData.TimeToEnableHitBox / Animator.GetNextAnimatorStateInfo(0).speed;
        float timeToDisableHitBox = MoveData.HitBoxData.TimeToDisableHitBox / Animator.GetNextAnimatorStateInfo(0).speed;
        StartTimerToEnableHitBox(timeToEnableHitBox, timeToDisableHitBox);
    }

    public override void Exit()
    {
        _isHitBoxTimerStarted = false;
    }

    private async void StartTimerToEnableHitBox(float timeToEnableHitBox, float timeToDisableHitBox)
    {
        await UniTask.Delay((int)(timeToEnableHitBox * 1000));
        HitBox.Enable();
        StartTimerToDisableHitBox(timeToDisableHitBox);
    }

    private async void StartTimerToDisableHitBox(float timeToDisableHitBox)
    {
        await UniTask.Delay((int)(timeToDisableHitBox * 1000));
        HitBox.Disable();
    }*/
}