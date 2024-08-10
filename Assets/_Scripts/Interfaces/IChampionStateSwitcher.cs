public interface IChampionStateSwitcher 
{
    void SwitchStateInstantly<T>() where T : ChampionBaseState;
    void StartTransitionToState<T>() where T : ChampionBaseState;
}