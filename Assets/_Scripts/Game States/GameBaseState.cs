public abstract class GameBaseState
{
    protected readonly GameStateMachine GameStateMachine;
    protected readonly IGameStateSwitcher GameStateSwitcher;
    protected readonly Champion FirstChampion;
    protected readonly Champion SecondChampion;

    public GameBaseState(GameStateMachine gameStateMachine, IGameStateSwitcher gameStateSwitcher, Champion firstChampion, Champion secondChampion)
    {
        GameStateMachine = gameStateMachine;
        GameStateSwitcher = gameStateSwitcher;
        FirstChampion = firstChampion;
        SecondChampion = secondChampion;
    }

    public abstract void Enter();
    public virtual void EnterClientServerRpc() { }
    public abstract void Update();
    public abstract void FixedUpdate();
    public abstract void LateUpdate();
    public abstract void Exit();
    public virtual void ExitClientServerRpc() { }
}