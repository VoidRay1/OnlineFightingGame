using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuView : BaseView
{
    [SerializeField] private Button _cancelFindingMatchButton;
    [SerializeField] private Button _oneVsOneOnlineButton;
    [SerializeField] private Button _trainingButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _quitButton;

    private Selectable _activeSelectedTab;
    private AudioClip _buttonClickedAudioClip;
    private GameCloser _gameCloser;

    public Button CancelFindingMatchButton => _cancelFindingMatchButton;
    public Button OneVsOneOnlineButton => _oneVsOneOnlineButton;

    public event Action OnCancelFindingMatch;
    public event Action OnOneVsOneOnlineClicked;
    public event Action OnShowSettingsMenu;

    public void Initialize(Selectable firstSelectedTab, GameCloser gameCloser)
    {
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        _activeSelectedTab = firstSelectedTab;
        _gameCloser = gameCloser;
        SubscribeOnEvents();
    }

    public override void Hide()
    {
        base.Hide();
        UnSubscribeOnEvents();
    }

    public void HideCancelFindingMatchButton()
    {
        _cancelFindingMatchButton.gameObject.SetActive(false);
        _activeSelectedTab = _oneVsOneOnlineButton;
        HighlightSelectedTab();
    }

    public void ShowCancelFindingMatchButton()
    {
        _cancelFindingMatchButton.gameObject.SetActive(true);
    }

    public void EnableGameButtons()
    {
        _oneVsOneOnlineButton.interactable = true;
        _trainingButton.interactable = true;
        _activeSelectedTab = _oneVsOneOnlineButton;
        SetSelectableOnUp(_settingsButton, _trainingButton);
        HighlightSelectedTab();
    }

    public void DisableGameButtons()
    {
        _oneVsOneOnlineButton.interactable = false;
        _trainingButton.interactable = false;
        _activeSelectedTab = _settingsButton;
        SetSelectableOnUp(_settingsButton, _cancelFindingMatchButton);
        HighlightSelectedTab();
    }

    public void HighlightSelectedTab()
    {
        _activeSelectedTab.Select();
    }

    private void SetSelectableOnUp(Selectable selectable, Selectable selectableOnUp)
    {
        Navigation navigation = selectable.navigation;
        navigation.selectOnUp = selectableOnUp;
        selectable.navigation = navigation;
    }

    private void CancelFindingMatch()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        OnCancelFindingMatch?.Invoke();
    }

    private void OneVsOne()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        OnOneVsOneOnlineClicked?.Invoke();
    }

    private void Training()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        SceneManager.LoadSceneAsync(Constants.Scenes.Training);
    }

    private void ShowSettingsMenu()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _activeSelectedTab = _settingsButton;
        OnShowSettingsMenu?.Invoke();
    }

    private void Quit()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _gameCloser.CloseGame();
    }

    private void SubscribeOnEvents()
    {
        _cancelFindingMatchButton.onClick.AddListener(CancelFindingMatch);
        _oneVsOneOnlineButton.onClick.AddListener(OneVsOne);
        _trainingButton.onClick.AddListener(Training);
        _settingsButton.onClick.AddListener(ShowSettingsMenu);
        _quitButton.onClick.AddListener(Quit);
    }

    private void UnSubscribeOnEvents()
    {
        _cancelFindingMatchButton.onClick.RemoveListener(CancelFindingMatch);
        _oneVsOneOnlineButton.onClick.RemoveListener(OneVsOne);
        _trainingButton.onClick.RemoveListener(Training);
        _settingsButton.onClick.RemoveListener(ShowSettingsMenu);
        _quitButton.onClick.RemoveListener(Quit);
    }
}