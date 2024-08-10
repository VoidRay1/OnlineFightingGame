using UnityEngine;

public class ChampionBackwardKnockoutState : ChampionBaseState
{
    public ChampionBackwardKnockoutState(Animator animator, IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions,
        Champion champion) : base(animator, championStateSwitcher, moveListActions, champion)
    {

    }

    public override void Enter()
    {
        Animator.SetTrigger(Constants.Animator.Params.KnockoutBackwardTrigger);
    }

    public override void Update()
    {

    }

    public override void LateUpdate()
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