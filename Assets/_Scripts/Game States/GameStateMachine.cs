using System;
using System.Collections.Generic;
using Unity.Netcode;

public class GameStateMachine : NetworkBehaviour, IGameStateSwitcher
{
    private Dictionary<Type, GameBaseState> _gameStates;
    private readonly Dictionary<int, GameBaseState> _indexGameStates = new Dictionary<int, GameBaseState>();
    private GameBaseState _activeState;

    public void SetGameStates(Dictionary<Type, GameBaseState> gameStates)
    {
        _gameStates = gameStates;
        int index = 0;
        foreach (var state in _gameStates)
        {
            _indexGameStates.Add(index, state.Value);
            index++;
        }
    }

    private void FixedUpdate()
    {
        _activeState?.FixedUpdate();
    }

    private void Update()
    {
        _activeState?.Update();
    }

    private void LateUpdate()
    {
        _activeState?.LateUpdate();
    }

    public void SwitchState<T>() where T : GameBaseState
    {
        if (_gameStates.ContainsKey(typeof(T)) == false)
        {
            throw new NullReferenceException("Game states doesn't have: " + typeof(T) + " state");
        }
        _activeState?.Exit();
        _activeState = _gameStates[typeof(T)];
        _activeState.Enter();
        for (int i = 0; i < _gameStates.Count; i++)
        {
            if (_indexGameStates[i] == _gameStates[typeof(T)])
            {
                SwitchStateClientRpc(i);
                break;
            }
        }
    }

    [ClientRpc]
    public void SwitchStateClientRpc(int index)
    {
        _activeState?.Exit();
        _activeState = _indexGameStates[index];
        _activeState.Enter();
    }

    [ClientRpc]
    public void EnterBeforeFightStateClientRpc()
    {
        var state = _gameStates[typeof(BeforeFightGameState)];
        state.EnterClientServerRpc();
    }

    [ClientRpc]
    public void ExitBeforeFightStateClientRpc() 
    {
        var state = _gameStates[typeof(BeforeFightGameState)];
        state.ExitClientServerRpc();
    }
}