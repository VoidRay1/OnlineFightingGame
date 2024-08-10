using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class ChampionStateTransition 
{
    private readonly ChampionBaseState _from;
    private readonly ChampionBaseState _to;
    private readonly Animator _animator;

    public Action<ChampionBaseState> OnTransitionToStateEnded;

    public ChampionStateTransition(ChampionBaseState from, ChampionBaseState to, Animator animator)
    {
        _from = from;
        _to = to;
        _animator = animator;
    }

    public async void StartTransition()
    {
        _from.Exit();
        await Task.Delay((int)(_animator.GetAnimatorTransitionInfo(Constants.Animator.Layers.BaseLayer).duration * 1000));
        OnTransitionToStateEnded?.Invoke(_to);
        _to.Enter();
    }
}