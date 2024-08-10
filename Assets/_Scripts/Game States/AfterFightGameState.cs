public class AfterFightGameState : GameBaseState
{
    private readonly FightTimer _fightTimer;

    public AfterFightGameState(GameStateMachine gameStateMachine, IGameStateSwitcher gameStateSwitcher, FightTimer fightTimer,
        Champion firstChampion, Champion secondChampion) : base(gameStateMachine, gameStateSwitcher, firstChampion, secondChampion)
    {
        _fightTimer = fightTimer;
    }

    public override void Enter()
    {
        _fightTimer.Stop();
        FirstChampion.StateMachine.OnChampionStateChanged += TrySwitchToBeforeFightState;
        SecondChampion.StateMachine.OnChampionStateChanged += TrySwitchToBeforeFightState;
    }

    public override void Update()
    {
        FirstChampion.StateMachine.SelfUpdate();
        SecondChampion.StateMachine.SelfUpdate();
    }

    public override void FixedUpdate()
    {
        FirstChampion.StateMachine.SelfFixedUpdate();
        SecondChampion.StateMachine.SelfFixedUpdate();
    }

    public override void LateUpdate()
    {
        FirstChampion.StateMachine.SelfLateUpdate();
        SecondChampion.StateMachine.SelfLateUpdate();
    }

    public override void Exit()
    {
        FirstChampion.StateMachine.OnChampionStateChanged -= TrySwitchToBeforeFightState;
        SecondChampion.StateMachine.OnChampionStateChanged -= TrySwitchToBeforeFightState;
    }

    private void TrySwitchToBeforeFightState(ChampionBaseState championState)
    {
        if (FirstChampion.StateMachine.ActiveState is ChampionIdleState && SecondChampion.StateMachine.ActiveState is ChampionIdleState)
        {
            GameStateSwitcher.SwitchState<BeforeFightGameState>();
        }
    }
}