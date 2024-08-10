using UnityEngine;
using UnityEngine.InputSystem;

public class ChampionInCrouchState : ChampionBaseState
{
    public ChampionInCrouchState(Animator animator, IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions,
        Champion champion) : base(animator, championStateSwitcher, moveListActions, champion)
    {

    }

    public override void Enter()
    {
        Animator.SetBool(Constants.Animator.Params.IsCharacterCrouching, true);
        MoveListActions.Uppercut.started += UppercutStarted;
        MoveListActions.Sweep.started += SweepStarted;
    }

    public override void Update()
    {
        if(MoveListActions.Crouch.phase != InputActionPhase.Performed)
        {
            Animator.SetBool(Constants.Animator.Params.IsCharacterCrouching, false);
            ChampionStateSwitcher.SwitchStateInstantly<ChampionIdleState>();
        }
    }

    public override void Exit()
    {
        MoveListActions.Uppercut.started -= UppercutStarted;
        MoveListActions.Sweep.started -= SweepStarted;
    }

    private void UppercutStarted(InputAction.CallbackContext obj)
    {
        ChampionStateSwitcher.SwitchStateInstantly<ChampionUppercutFromCrouchState>();
    }

    private void SweepStarted(InputAction.CallbackContext obj)
    {
        ChampionStateSwitcher.SwitchStateInstantly<ChampionSweepState>();
    }
}