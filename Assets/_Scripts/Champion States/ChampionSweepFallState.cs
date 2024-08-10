using UnityEngine;

public class ChampionSweepFallState : ChampionBaseState
{
    public ChampionSweepFallState(Animator animator, IChampionStateSwitcher championStateSwitcher,
        GameControls.MoveListActions moveListActions, Champion champion) : base(animator, championStateSwitcher, moveListActions, champion)
    {

    }

    public override void Enter()
    {
        Animator.SetTrigger(Constants.Animator.Params.SweepFallTrigger);
    }

    public override void Update()
    {
        if (Animator.IsInTransition(Constants.Animator.Layers.BaseLayer) == false
            && Animator.GetCurrentAnimatorStateInfo(Constants.Animator.Layers.BaseLayer).IsName(Constants.Animator.Names.StandUp))
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionStandUpState>();
        }
    }

    public override void Exit()
    {
        
    }
}