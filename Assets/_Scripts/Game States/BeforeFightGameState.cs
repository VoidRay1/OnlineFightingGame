using System.Collections.Generic;
using Unity.Netcode;

public class BeforeFightGameState : GameBaseState
{
    private readonly FightTimer _fightTimer;
    private readonly BeforeFightActionsDisplayer _beforeFightActionsDisplayer;
    private GameControls.MoveListActions MoveListActions => GameContext.Instance.GameControls.MoveList;
    private readonly byte _fightRounds;
    private byte _currentFightRound;

    public BeforeFightGameState(GameStateMachine gameStateMachine, IGameStateSwitcher gameStateSwitcher, FightTimer fightTimer, Champion firstChampion, Champion secondChampion,
        BeforeFightActionsDisplayer beforeFightActionsDisplayer, byte fightRounds) : base(gameStateMachine, gameStateSwitcher, firstChampion, secondChampion)
    {
        _fightTimer = fightTimer;   
        _beforeFightActionsDisplayer = beforeFightActionsDisplayer;
        _fightRounds = fightRounds;
    }

    public override void Enter()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            GameStateMachine.EnterBeforeFightStateClientRpc();
            EnterClientServerRpc();
            _beforeFightActionsDisplayer.OnBeforeFightActionsEnded += SwitchToFightState;
        }
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        
    }

    public override void LateUpdate()
    {

    }

    public override void Exit()
    {
        if (NetworkManager.Singleton.IsServer)
        {
            GameStateMachine.ExitBeforeFightStateClientRpc();
            ExitClientServerRpc();
            _beforeFightActionsDisplayer.OnBeforeFightActionsEnded -= SwitchToFightState;
        }
    }

    private void SwitchToFightState()
    {
        GameStateMachine.SwitchState<FightGameState>();
    }

    private string GetRound(byte currentFightRound, byte fightRounds)
    {
        return currentFightRound != fightRounds ? $"ROUND {currentFightRound}" : "FINAL ROUND";
    }

    public override void EnterClientServerRpc()
    {
        _currentFightRound++;
        _fightTimer.ResetTimer();
        FirstChampion.ResetStats();
        SecondChampion.ResetStats();
        _beforeFightActionsDisplayer.DisplayBeforeFightPreparationsTexts(new Queue<string>
        (
            new string[]
            {
                GetRound(_currentFightRound, (byte)(_fightRounds + 1)),
                "FIGHT"
            }
        ));
    }

    public override void ExitClientServerRpc()
    {
        _fightTimer.Run();
        MoveListActions.Enable();
    }
}