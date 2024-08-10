using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OneVsOneOnlineGame : Game
{
    [SerializeField] private BeforeFightActionsDisplayer _beforeFightActionsDisplayer;
    [SerializeField] private FightRoundTogglesCreator _fightRoundTogglesCreator;
    [SerializeField] private FightTimer _fightTimer;
    [SerializeField, Range(1, 3)] private byte _fightRounds = 2;

    private void Start()
    {
        if (IsServer)
        {
            if (NetworkManager.Singleton.ConnectedClients.Count < Constants.MaxPlayers)
            {
                NetworkManager.Singleton.OnClientConnectedCallback += ClientConnected;
            }
            else
            {
                PrepareGame();
            }
        }
    }

    private void ClientConnected(ulong clientId)
    {
        if (NetworkManager.Singleton.ConnectedClients.Count == Constants.MaxPlayers)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= ClientConnected;
            PrepareGame();
        }
    }

    private void PrepareGame()
    {
        FirstChampion = ChampionSpawner.SpawnClientChampion(FirstChampionType, NetworkManager.Singleton.ConnectedClientsIds[0], true);
        SecondChampion = ChampionSpawner.SpawnClientChampion(SecondChampionType, NetworkManager.Singleton.ConnectedClientsIds[1], false);
        SetChampionsClientRpc(FirstChampion, SecondChampion);
        InitializeGame();
        InitializeClientRpc();
    }

    [ClientRpc]
    private void SetChampionsClientRpc(NetworkBehaviourReference firstChampion, NetworkBehaviourReference secondChampion)
    {
        if (firstChampion.TryGet(out Champion firstClientChampion))
        {
            FirstChampion = firstClientChampion;
        }
        if (secondChampion.TryGet(out Champion secondClientChampion))
        {
            SecondChampion = secondClientChampion;
        }
    }

    [ClientRpc]
    private void InitializeClientRpc()
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        FirstChampion.Initialize(SecondChampion.ChampionHead, FirstChampionHud, GameControls.MoveList, FirstChampion.IsOwner);
        SecondChampion.Initialize(FirstChampion.ChampionHead, SecondChampionHud, GameControls.MoveList, SecondChampion.IsOwner);
        _fightRoundTogglesCreator.CreateFightRoundToggles(_fightRounds);
        CameraMover.Initialize(FirstChampion.transform, SecondChampion.transform);
        GameStateMachine.SetGameStates(new Dictionary<Type, GameBaseState>
        {
            [typeof(BeforeFightGameState)] =
                new BeforeFightGameState(GameStateMachine, GameStateMachine, _fightTimer, FirstChampion, SecondChampion, _beforeFightActionsDisplayer, _fightRounds),
            [typeof(FightGameState)] = new FightGameState(GameStateMachine, GameStateMachine, FirstChampion, SecondChampion),
            [typeof(AfterFightGameState)] = new AfterFightGameState(GameStateMachine, GameStateMachine, _fightTimer, FirstChampion, SecondChampion)
        });
        GameStateMachine.SwitchState<BeforeFightGameState>();
    }
}