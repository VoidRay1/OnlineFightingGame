using UnityEngine;
using UnityEngine.InputSystem;

public class ChampionSweepState : ChampionAttackState
{
    private readonly ChampionTracker _championTracker;

    public ChampionSweepState(Animator animator, IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions,
        Champion champion, Transform hitBoxParent) 
        : base(animator, championStateSwitcher, moveListActions, champion, hitBoxParent, AttackType.Sweep)
    {
        _championTracker = champion.Tracker;
    }

    public override void Enter()
    {
        _championTracker.Disable();
        Animator.SetTrigger(Constants.Animator.Params.SweepTrigger);
        base.Enter();
        MoveListActions.Crouch.canceled += CrouchCanceled;
    }

    public override void Update()
    {
       
    }

    public override void LateUpdate()
    {
        if (Animator.IsInTransition(Constants.Animator.Layers.BaseLayer) == false 
            && Animator.GetCurrentAnimatorStateInfo(Constants.Animator.Layers.BaseLayer).IsName(Constants.Animator.Names.Idle))
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionIdleState>();
        }
        if (Animator.IsInTransition(Constants.Animator.Layers.BaseLayer) == false 
            && Animator.GetCurrentAnimatorStateInfo(Constants.Animator.Layers.BaseLayer).IsName(Constants.Animator.Names.Crouch))
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionInCrouchState>();
        }
    }

    public override void Exit()
    {
        _championTracker.Enable();
        MoveListActions.Crouch.canceled -= CrouchCanceled;
    }

    private void CrouchCanceled(InputAction.CallbackContext obj)
    {
        Animator.SetBool(Constants.Animator.Params.IsCharacterCrouching, false);
    }
}