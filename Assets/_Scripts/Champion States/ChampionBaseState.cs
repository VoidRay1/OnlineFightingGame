using UnityEngine;

public abstract class ChampionBaseState 
{
    protected readonly GameControls.MoveListActions MoveListActions;
    protected readonly Animator Animator;
    protected readonly IChampionStateSwitcher ChampionStateSwitcher;
    protected readonly Champion Champion;

    public ChampionBaseState(Animator animator, IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions, Champion champion)
    {
        MoveListActions = moveListActions;
        Animator = animator;
        ChampionStateSwitcher = championStateSwitcher;
        Champion = champion;
    }

    public abstract void Enter();
    public virtual void FixedUpdate() { }
    public abstract void Update();
    public virtual void LateUpdate() { }
    public abstract void Exit();
}