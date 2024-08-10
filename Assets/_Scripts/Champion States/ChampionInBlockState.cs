using UnityEngine;
using UnityEngine.InputSystem;

public class ChampionInBlockState : ChampionBaseState
{
    public ChampionInBlockState(Animator animator, IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions,
        Champion champion) : base(animator, championStateSwitcher, moveListActions, champion)
    {

    }

    public override void Enter()
    {
        MoveListActions.Block.canceled += BlockCanceled;
        Animator.SetBool(Constants.Animator.Params.IsCharacterInBlock, true);
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        MoveListActions.Block.canceled -= BlockCanceled;
    }

    private void BlockCanceled(InputAction.CallbackContext obj)
    {
        Animator.SetBool(Constants.Animator.Params.IsCharacterInBlock, false);
        ChampionStateSwitcher.StartTransitionToState<ChampionIdleState>();
    }
}
