using UnityEngine;

public class ChampionStandUpState : ChampionBaseState
{
    public ChampionStandUpState(Animator animator, IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions, Champion champion) 
        : base(animator, championStateSwitcher, moveListActions, champion)
    {

    }

    public override void Enter()
    {
        
    }

    public override void Update()
    {
        if (Animator.IsInTransition(Constants.Animator.Layers.BaseLayer) == false 
            && Animator.GetCurrentAnimatorStateInfo(Constants.Animator.Layers.BaseLayer).IsName(Constants.Animator.Names.Idle))
        {
            ChampionStateSwitcher.SwitchStateInstantly<ChampionIdleState>();
        }
    }

    public override void Exit()
    {
        
    }
}