using Cysharp.Threading.Tasks;
using System;
using UnityEngine;

public abstract class ChampionStaminaState : ChampionBaseState
{
    private readonly Stamina _stamina;
    private readonly float _staminaUsagePerSecond;
    private bool _useStamina;

    protected event Action OnStaminaEnded;

    public ChampionStaminaState(Animator animator, IChampionStateSwitcher championStateSwitcher, GameControls.MoveListActions moveListActions,
        Champion champion, float staminaUsagePerSecond) 
        : base(animator, championStateSwitcher, moveListActions, champion)
    {
        _stamina = champion.Stamina;
        _staminaUsagePerSecond = staminaUsagePerSecond;
    }

    public override void Enter()
    {
        UseStamina();
    }

    public override void Exit()
    {
        _useStamina = false;
    }

    public async void UseStamina()
    {
        _useStamina = true;
        while (_useStamina && _stamina.IsStaminaEnded() == false)
        {
            _stamina.Spend(_staminaUsagePerSecond);
            if (_stamina.IsStaminaEnded())
            {
                _useStamina = false;
                OnStaminaEnded?.Invoke();
                break;
            }
            await UniTask.WaitForSeconds(1.0f);
        }
    }
}