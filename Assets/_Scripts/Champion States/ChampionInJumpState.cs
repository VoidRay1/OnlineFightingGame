using UnityEngine;

public class ChampionInJumpState : ChampionBaseState
{
    private readonly JumpController _jumpController;

    public ChampionInJumpState(Animator animator, JumpController jumpController, IChampionStateSwitcher championStateSwitcher,
        GameControls.MoveListActions moveListActions, Champion champion) : base(animator, championStateSwitcher, moveListActions, champion) 
    {
        _jumpController = jumpController;
    }

    public override void Enter()
    {
        _jumpController.ResetJumpVelocity();
        Animator.SetTrigger(Constants.Animator.Params.JumpTrigger);
        Animator.SetBool(Constants.Animator.Params.IsCharacterGrounded, false);
    }

    public override void FixedUpdate()
    {
        _jumpController.ApplyJumpVelocity();
    }

    public override void Update()
    {
        if (_jumpController.IsGroundedByRaycast)
        {
            Animator.SetBool(Constants.Animator.Params.IsCharacterGrounded, true);
            ChampionStateSwitcher.StartTransitionToState<ChampionLandingState>();
        }
    }

    public override void Exit()
    {
       
    }
}