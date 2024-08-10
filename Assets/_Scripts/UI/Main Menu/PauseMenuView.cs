using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuView : BaseView
{
    [SerializeField] private Button _moveListButton;
    [SerializeField] private Button _mainMenuButton;

    private readonly MoveListWindowProvider _moveListWindowProvider = new MoveListWindowProvider();
    private IReadOnlyList<AttackMoveData> _attackMovesData;

    public bool IsOpened => Canvas.enabled;

    public void Initialize(IReadOnlyList<AttackMoveData> attackMovesData)
    {
        _attackMovesData = attackMovesData;
    }

    public override void Show()
    {
        _moveListButton.onClick.AddListener(MoveListButtonClicked);
        _mainMenuButton.onClick.AddListener(MainMenuButtonClicked);
        _moveListButton.Select();
        base.Show();
    }

    private void MainMenuButtonClicked()
    {
        SceneManager.LoadSceneAsync(Constants.Scenes.MainMenu);
    }

    private void MoveListButtonClicked()
    {
        _moveListWindowProvider.ShowMoveListWindow(_attackMovesData);
    }

    private void OnDestroy()
    {
        _moveListButton.onClick.RemoveListener(MoveListButtonClicked);
        _mainMenuButton.onClick.RemoveListener(MainMenuButtonClicked);
    }
}