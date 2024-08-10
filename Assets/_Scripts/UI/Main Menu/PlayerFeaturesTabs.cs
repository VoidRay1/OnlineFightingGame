using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFeaturesTabs : MultiTabsView
{
    [SerializeField] private Button _playerDetailProfileButton;
    [SerializeField] private Button _friendsButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private PlayerDetailProfileTab _playerDetailProfileTab;
    [SerializeField] private FriendTab _friendTab;

    private AudioClip _buttonClickedAudioClip;

    public PlayerDetailProfileTab PlayerDetailProfileTab => _playerDetailProfileTab;
    public FriendTab FriendsTab => _friendTab;

    private void Awake()
    {
        Canvas.enabled = false;
    }

    public void Initialize(IReadOnlyList<AvatarData> avatarsData)
    {
        SetTabs(new List<TabView> { _playerDetailProfileTab, _friendTab });
        HideTabs();
        _friendTab.Initialize();
        _playerDetailProfileTab.Initialize(avatarsData);
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        SubscribeOnEvents();
    }

    public void ShowPlayerDetailProfileTab()
    {
        ShowTabWithNavigationOnDown(_playerDetailProfileTab);
        base.Show();
    }

    public void ShowFriendsTab()
    {
        ShowTabWithNavigationOnDown(_friendTab);
        base.Show();
    }

    private void ShowPlayerDetailProfileTabWithSound()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        ShowPlayerDetailProfileTab();
    }

    private void ShowFriendsTabWithSound()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        ShowFriendsTab();
    }

    public override void Hide()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        base.Hide();
    }

    private void ClosePlayerFeaturesTabs()
    {
        Hide();
    }

    private void SubscribeOnEvents()
    {
        _playerDetailProfileButton.onClick.AddListener(ShowPlayerDetailProfileTabWithSound);
        _friendsButton.onClick.AddListener(ShowFriendsTabWithSound);
        _closeButton.onClick.AddListener(ClosePlayerFeaturesTabs);
    }

    private void UnSubscribeOnEvents()
    {
        _playerDetailProfileButton.onClick.RemoveListener(ShowPlayerDetailProfileTabWithSound);
        _friendsButton.onClick.RemoveListener(ShowFriendsTabWithSound);
        _closeButton.onClick.RemoveListener(ClosePlayerFeaturesTabs);
    }

    private void OnDestroy()
    {
        UnSubscribeOnEvents();
    }
}