using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ChampionStateMachine : NetworkBehaviour, IChampionStateSwitcher
{
    [SerializeField] private Transform _leftArmTransform;
    [SerializeField] private Transform _rightLegTransform;
    [SerializeField] private MovesList _movesList;
    [SerializeField] private HitBox _hitBox;
    [SerializeField] private MoveController _moveController;
    [SerializeField] private JumpController _jumpController;
    [SerializeField] private Animator _animator;

    private Dictionary<Type, ChampionBaseState> _championStates;
    private Dictionary<int, ChampionBaseState> _indexChampionStates = new Dictionary<int, ChampionBaseState>();
    private List<Type> _championAttackTypes;
    private ChampionStateDisplayer _currentChampionStateDisplayer;
    private ChampionBaseState _activeState;
    private Champion _champion;

    public ChampionBaseState ActiveState => _activeState;
    public event Action<ChampionBaseState> OnChampionStateChanged;

    public void Initialize(GameControls.MoveListActions moveListActions, Champion champion)
    {
        _champion = champion;
        _currentChampionStateDisplayer = champion.Hud.ChampionStateDisplayer;
        _championStates = new Dictionary<Type, ChampionBaseState>()
        {
            [typeof(ChampionEmptyState)] = new ChampionEmptyState(_animator, this, moveListActions, champion),
            [typeof(ChampionIdleState)] = new ChampionIdleState(_animator, this, moveListActions, champion),
            [typeof(ChampionInCrouchState)] = new ChampionInCrouchState(_animator, this, moveListActions, champion),
            [typeof(ChampionInBlockState)] = new ChampionInBlockState(_animator, this, moveListActions, champion),
            [typeof(ChampionBackwardStepState)] = new ChampionBackwardStepState(_animator, _moveController, this, moveListActions, champion),
            [typeof(ChampionForwardStepState)] = new ChampionForwardStepState(_animator, _moveController, this, moveListActions, champion),
            [typeof(ChampionMoveBackwardState)] = new ChampionMoveBackwardState(_animator, _moveController, this, moveListActions, champion),
            [typeof(ChampionMoveForwardState)] = new ChampionMoveForwardState(_animator, _moveController, this, moveListActions, champion),
            [typeof(ChampionRunBackwardState)] = new ChampionRunBackwardState(_animator, this, moveListActions, _moveController, champion),
            [typeof(ChampionRunForwardState)] = new ChampionRunForwardState(_animator, this, moveListActions, _moveController, champion),
            [typeof(ChampionInJumpState)] = new ChampionInJumpState(_animator, _jumpController, this, moveListActions, champion),
            [typeof(ChampionLandingState)] = new ChampionLandingState(_animator, this, moveListActions, champion),
            [typeof(ChampionUppercutFromCrouchState)] = new ChampionUppercutFromCrouchState(_animator, this, moveListActions, champion, _leftArmTransform),
            [typeof(ChampionPunchState)] = new ChampionPunchState(_animator, this, moveListActions, champion, _leftArmTransform),
            [typeof(ChampionSweepState)] = new ChampionSweepState(_animator, this, moveListActions, champion, _rightLegTransform),
            [typeof(ChampionUppercutFromIdleState)] = new ChampionUppercutFromIdleState(_animator, this, moveListActions, champion, _leftArmTransform),
            [typeof(ChampionLegKickState)] = new ChampionLegKickState(_animator, this, moveListActions, champion, _rightLegTransform),
            [typeof(ChampionReceivePunchState)] = new ChampionReceivePunchState(_animator, this, moveListActions, champion),
            [typeof(ChampionReceiveUppercutState)] = new ChampionReceiveUppercutState(_animator, this, moveListActions, champion),
            [typeof(ChampionSweepFallState)] = new ChampionSweepFallState(_animator, this, moveListActions, champion),
            [typeof(ChampionBackwardKnockoutState)] = new ChampionBackwardKnockoutState(_animator, this, moveListActions, champion),
            [typeof(ChampionStandUpState)] = new ChampionStandUpState(_animator, this, moveListActions, champion),
        };
        int index = 0;
        foreach (var state in _championStates)
        {
            _indexChampionStates.Add(index, state.Value);
            index++;
        }
        _championAttackTypes = new List<Type>()
        {
            typeof(ChampionPunchState),
            typeof(ChampionUppercutFromIdleState),
            typeof(ChampionUppercutFromCrouchState),
            typeof(ChampionSweepState),
            typeof(ChampionLegKickState)
        };
        SetMoveDataToAttackStates();
        if (_champion.IsActive)
        {
            SwitchStateInstantly<ChampionIdleState>();
        }
    }

    public void SelfFixedUpdate() 
    {
        if (_champion.IsActive)
        {
            _activeState?.FixedUpdate();
        }
    }

    public void SelfUpdate()
    {
        if (_champion.IsActive)
        {
            _activeState?.Update();
        }
    }

    public void SelfLateUpdate()
    {
        if (_champion.IsActive)
        {
            _activeState?.LateUpdate();
        }
    }

    public void SwitchStateInstantly<T>() where T : ChampionBaseState
    {
        _activeState?.Exit();
        _activeState = _championStates[typeof(T)];
        _activeState.Enter();
        //_currentChampionStateDisplayer.DisplayCurrentState(_activeState.ToString());
        OnChampionStateChanged?.Invoke(_activeState);
        for (int i = 0; i < _championStates.Count; i++)
        {
            if (_indexChampionStates[i] == _championStates[typeof(T)])
            {
                SwitchStateInstantlyServerRpc(NetworkManager.Singleton.LocalClientId, i);
                break;
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void SwitchStateInstantlyServerRpc(ulong clientId, int index)
    {
        SwitchStateInstantlyClientRpc(clientId, index);
    }

    [ClientRpc]
    private void SwitchStateInstantlyClientRpc(ulong clientId, int index)
    {
        if (clientId == NetworkManager.LocalClientId)
        {
            return;
        }
        SwitchStateInstantlyByIndex(index);
    }

    private void SwitchStateInstantlyByIndex(int index)
    {
        if (_champion.IsActive)
        {
            return;
        }
        _activeState?.Exit();
        _activeState = _indexChampionStates[index];
        _activeState.Enter();
        //_currentChampionStateDisplayer.DisplayCurrentState(_activeState.ToString());
        OnChampionStateChanged?.Invoke(_activeState);
    }

    [ServerRpc(RequireOwnership = false)]
    public void StartTransitionToStateServerRpc(ulong clientId, int index)
    {
        StartTransitionToStateByIndexClientRpc(clientId, index);
    }

    [ClientRpc]
    private void StartTransitionToStateByIndexClientRpc(ulong clientId, int index)
    {
        if (clientId == NetworkManager.LocalClientId)
        {
            return;
        }
        StartTransitionToStateByIndex(index);
    }

    private void StartTransitionToStateByIndex(int index)
    {
        if (_champion.IsActive)
        {
            return;
        }
        ChampionStateTransition championStateTransition = new ChampionStateTransition(_activeState, _indexChampionStates[index], _animator);
        championStateTransition.OnTransitionToStateEnded += SetCurrentState;
        //_currentChampionStateDisplayer.DisplayTransitionBetweenStates(_activeState.ToString(), _indexChampionStates[index].ToString());
        _activeState = null;
        StartCoroutine(WaitForEndOfFrame(championStateTransition));
    }

    public void StartTransitionToState<T>() where T : ChampionBaseState
    {
        ChampionStateTransition championStateTransition = new ChampionStateTransition(_activeState, _championStates[typeof(T)], _animator);
        championStateTransition.OnTransitionToStateEnded += SetCurrentState;
        //_currentChampionStateDisplayer.DisplayTransitionBetweenStates(_activeState.ToString(), _championStates[typeof(T)].ToString());
        _activeState = null;
        StartCoroutine(WaitForEndOfFrame(championStateTransition));
        for (int i = 0; i < _championStates.Count; i++)
        {
            if (_indexChampionStates[i] == _championStates[typeof(T)])
            {
                StartTransitionToStateServerRpc(NetworkManager.Singleton.LocalClientId, i);
                break;
            }
        }
    }

    private void SetCurrentState(ChampionBaseState state)
    {
        _activeState = state;
        //_currentChampionStateDisplayer.DisplayCurrentState(_activeState.ToString());
        OnChampionStateChanged?.Invoke(_activeState);
    }

    private IEnumerator WaitForEndOfFrame(ChampionStateTransition championStateTransition)
    {
        yield return null;
        championStateTransition.StartTransition();
    }

    private void SetMoveDataToAttackStates()
    {
        for (int i = 0; i < _championAttackTypes.Count; i++)
        {
            ChampionAttackState attackState = _championStates[_championAttackTypes[i]] as ChampionAttackState;
            for (int j = 0; j < _movesList.Moves.Count; j++)
            {
                if (attackState.AttackType == _movesList.Moves[j].AttackType)
                {
                    attackState.SetMoveData(_movesList.Moves[j]);
                    break;
                }
            }
        }
    }
}