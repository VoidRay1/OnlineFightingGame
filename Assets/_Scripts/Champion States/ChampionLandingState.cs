using UnityEngine;

public class ChampionLandingState : ChampionBaseState
{
    public ChampionLandingState(Animator animator, IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions,
        Champion champion) : base(animator, championStateSwitcher, moveListActions, champion) 
    { 

    }

    public override void Enter()
    {
        ChampionStateSwitcher.StartTransitionToState<ChampionIdleState>();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {

    }
}