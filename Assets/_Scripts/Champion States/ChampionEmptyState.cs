using UnityEngine;

public class ChampionEmptyState : ChampionBaseState
{
    public ChampionEmptyState(Animator animator, IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions,
        Champion champion) : base(animator, championStateSwitcher, moveListActions, champion)
    {
    }

    public override void Enter()
    {
        
    }

    public override void Exit()
    {
        
    }

    public override void Update()
    {
        
    }
}