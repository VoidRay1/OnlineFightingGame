using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Game : NetworkBehaviour
{
    [SerializeField] private MovesList _movesList;
    [SerializeField] private CameraMover _cameraMover;
    [SerializeField] private GameStateMachine _gameStateMachine;
    [SerializeField] private ChampionHud _firstChampionHud;
    [SerializeField] private ChampionHud _secondChampionHud;
    [SerializeField] private ChampionSpawner _championsSpawner;
    [SerializeField] private ChampionType _firstChampionType;
    [SerializeField] private ChampionType _secondChampionType;

    private PauseMenuProvider _pauseMenuProvider;

    [NonSerialized] public Champion FirstChampion;
    [NonSerialized] public Champion SecondChampion;

    public CameraMover CameraMover => _cameraMover;
    public GameStateMachine GameStateMachine => _gameStateMachine;
    public ChampionHud FirstChampionHud => _firstChampionHud;
    public ChampionHud SecondChampionHud => _secondChampionHud;
    public ChampionSpawner ChampionSpawner => _championsSpawner;
    public ChampionType FirstChampionType => _firstChampionType;
    public ChampionType SecondChampionType => _secondChampionType;
    public GameControls GameControls => GameContext.Instance.GameControls;
    
    public virtual void Initialize()
    {
        _pauseMenuProvider = new PauseMenuProvider();
        GameControls.Window.Close.started += TriggerPauseMenu;
    }

    private void TriggerPauseMenu(InputAction.CallbackContext obj)
    {
        if (_pauseMenuProvider.IsPauseMenuOpened)
        {
            _pauseMenuProvider.ClosePauseMenu();
            GameControls.MoveList.Enable();
        }
        else
        {
            _pauseMenuProvider.ShowPauseMenu(_movesList.Moves);
            GameControls.MoveList.Disable();
        }
    }

    private new void OnDestroy()
    {
        GameControls.Window.Close.started -= TriggerPauseMenu;
    }
}