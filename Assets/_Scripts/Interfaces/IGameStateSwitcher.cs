public interface IGameStateSwitcher 
{
    void SwitchState<T>() where T : GameBaseState;
}