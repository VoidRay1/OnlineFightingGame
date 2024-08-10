using System.Collections.Generic;

public class PauseMenuProvider : AddressableLoader
{
    private PauseMenuView _pauseMenu;
    public bool IsPauseMenuOpened => _pauseMenu != null && _pauseMenu.IsOpened;

    public async void ShowPauseMenu(IReadOnlyList<AttackMoveData> attackMovesData)
    {
        _pauseMenu = await Load<PauseMenuView>(Constants.Addressables.Keys.PauseMenu);
        _pauseMenu.Initialize(attackMovesData);
        _pauseMenu.Show();
    }

    public void ClosePauseMenu()
    {
        _pauseMenu = null;
        UnloadCachedGameObject();
    }
}