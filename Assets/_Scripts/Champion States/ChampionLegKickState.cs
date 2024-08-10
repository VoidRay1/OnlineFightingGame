using UnityEngine;

public class ChampionLegKickState : ChampionAttackState
{
    public ChampionLegKickState(Animator animator, IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions,
        Champion champion, Transform hitBoxParent) 
        : base(animator, championStateSwitcher, moveListActions, champion, hitBoxParent, AttackType.LegKick)
    {

    }

    public override void Enter()
    {
        Animator.SetTrigger(Constants.Animator.Params.LegKickTrigger);
        base.Enter();
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
    }

    public override void Exit()
    {
        
    }
}