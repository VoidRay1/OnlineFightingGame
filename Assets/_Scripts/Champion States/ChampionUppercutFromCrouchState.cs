using UnityEngine;
using UnityEngine.InputSystem;

public class ChampionUppercutFromCrouchState : ChampionAttackState
{ 
    public ChampionUppercutFromCrouchState(Animator animator, IChampionStateSwitcher championStateSwitcher,
        GameControls.MoveListActions moveListActions, Champion champion, Transform hitBoxParent)
        : base(animator, championStateSwitcher, moveListActions, champion, hitBoxParent, AttackType.Uppercut)
    {

    }

    public override void Enter()
    {
        Animator.SetLayerWeight(Constants.Animator.Layers.UpperBodyLayer, 1.0f);
        Animator.SetBool(Constants.Animator.Params.IsCharacterUsingUppercutFromCrouch, true);
        Animator.SetTrigger(Constants.Animator.Params.UppercutFromCrouchTrigger);
        base.Enter();
        MoveListActions.Crouch.started += CrouchStarted;
        MoveListActions.Crouch.canceled += CrouchCanceled;
    }

    public override void Update()
    {
        
    }

    public override void LateUpdate()
    {
        if (Animator.IsInTransition(Constants.Animator.Layers.UpperBodyLayer) == false 
            && Animator.GetCurrentAnimatorStateInfo(Constants.Animator.Layers.UpperBodyLayer).IsName(Constants.Animator.Names.Empty) 
            && Animator.GetBool(Constants.Animator.Params.IsCharacterCrouching))
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionInCrouchState>();
        }
        if(Animator.IsInTransition(Constants.Animator.Layers.UpperBodyLayer) == false
            && Animator.GetCurrentAnimatorStateInfo(Constants.Animator.Layers.UpperBodyLayer).IsName(Constants.Animator.Names.Empty)
            && Animator.GetBool(Constants.Animator.Params.IsCharacterCrouching) == false)
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionIdleState>();
        }
    }

    public override void Exit()
    {
        Animator.SetBool(Constants.Animator.Params.IsCharacterUsingUppercutFromCrouch, false);
        Animator.SetLayerWeight(Constants.Animator.Layers.UpperBodyLayer, 0.0f);
        MoveListActions.Crouch.started -= CrouchStarted;
        MoveListActions.Crouch.canceled -= CrouchCanceled;
    }

    private void CrouchStarted(InputAction.CallbackContext obj)
    {
        Animator.SetBool(Constants.Animator.Params.IsCharacterCrouching, true);
    }

    private void CrouchCanceled(InputAction.CallbackContext obj)
    {
        Animator.SetBool(Constants.Animator.Params.IsCharacterCrouching, false);
    }
}