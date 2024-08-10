public class TrainingFightGameState : GameBaseState
{
    private GameControls.MoveListActions MoveListActions => GameContext.Instance.GameControls.MoveList;
    
    public TrainingFightGameState(GameStateMachine gameStateMachine, IGameStateSwitcher gameStateSwitcher, Champion firstChampion, Champion secondChampion) 
        : base(gameStateMachine, gameStateSwitcher, firstChampion, secondChampion)
    {

    }

    public override void Enter()
    {
        MoveListActions.Enable();
        FirstChampion.Tracker.Enable();
        SecondChampion.Tracker.Enable();
        SecondChampion.Health.OnHealthEnded += ResetSecondChampionStats;
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
        SecondChampion.Health.OnHealthEnded -= ResetSecondChampionStats;
    }

    private void ResetSecondChampionStats()
    {
        SecondChampion.ResetStats();
    }
}