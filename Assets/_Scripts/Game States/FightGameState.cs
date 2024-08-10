public class FightGameState : GameBaseState
{
    private GameControls.MoveListActions MoveListActions => GameContext.Instance.GameControls.MoveList;

    public FightGameState(GameStateMachine gameStateMachine, IGameStateSwitcher gameStateSwitcher, Champion firstChampion, Champion secondChampion) 
        : base(gameStateMachine, gameStateSwitcher, firstChampion, secondChampion)
    {

    }

    public override void Enter()
    {
        FirstChampion.Tracker.Enable();
        SecondChampion.Tracker.Enable();
        FirstChampion.Health.OnHealthEnded += SecondChampionWonRound;
        SecondChampion.Health.OnHealthEnded += FirstChampionWonRound;
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
        MoveListActions.Disable();
        FirstChampion.Health.OnHealthEnded -= SecondChampionWonRound;
        SecondChampion.Health.OnHealthEnded -= FirstChampionWonRound;
    }

    private void FirstChampionWonRound()
    {
        FirstChampion.Hud.FightRoundsToggler.TurnOnTheRoundWinSwitch();
        SecondChampion.StateMachine.SwitchStateInstantly<ChampionBackwardKnockoutState>();
        DisableTrackers();
        GameStateSwitcher.SwitchState<AfterFightGameState>();
    }

    private void SecondChampionWonRound()
    {
        SecondChampion.Hud.FightRoundsToggler.TurnOnTheRoundWinSwitch();
        FirstChampion.StateMachine.SwitchStateInstantly<ChampionBackwardKnockoutState>();
        DisableTrackers();
        GameStateSwitcher.SwitchState<AfterFightGameState>();
    }

    private void DisableTrackers()
    {
        FirstChampion.Tracker.Disable();
        SecondChampion.Tracker.Disable();
    }
}