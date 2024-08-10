using System;
using System.Collections.Generic;

public class TrainingGame : Game
{
    private void Start()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();
        (FirstChampion, SecondChampion) = ChampionSpawner.Spawn(FirstChampionType, SecondChampionType);
        FirstChampion.Initialize(SecondChampion.ChampionHead, FirstChampionHud, GameControls.MoveList, true);
        SecondChampion.Initialize(FirstChampion.ChampionHead, SecondChampionHud, GameControls.MoveList, false);
        CameraMover.Initialize(FirstChampion.transform, SecondChampion.transform);
        GameStateMachine.SetGameStates(new Dictionary<Type, GameBaseState>
        {
            [typeof(TrainingFightGameState)] = new TrainingFightGameState(GameStateMachine, GameStateMachine, FirstChampion, SecondChampion)
        });
        GameStateMachine.SwitchState<TrainingFightGameState>();
    }
}