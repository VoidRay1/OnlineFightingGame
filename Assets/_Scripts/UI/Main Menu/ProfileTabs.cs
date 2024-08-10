using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ProfileTabs : MultiTabsView
{
    [SerializeField] private Button _avatarsButton;
    [SerializeField] private AvatarsTab _avatarsTab;

    private AudioClip _buttonClickedAudioClip;

    public AvatarsTab AvatarsTab => _avatarsTab;

    public void Initialize(IReadOnlyList<AvatarData> avatarsData)
    {
        SetTabs(new List<TabView> { _avatarsTab });
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        _avatarsTab.RefreshAvatarsViews(_avatarsButton, avatarsData);
        SubscribeOnEvents();
        HideTabs();
    }

    public override void Show()
    {
        SubscribeOnEvents();
        ShowTabWithoutNavigationOnDown(_avatarsTab);
        base.Show();
    }

    public override void Hide()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        base.Hide();
        UnSubscribeOnEvents();
    }

    private void ShowAvatarsTab()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        ShowTabWithNavigationOnDown(_avatarsTab);
    }

    private void SubscribeOnEvents()
    {
        _avatarsButton.onClick.AddListener(ShowAvatarsTab);
    }

    private void UnSubscribeOnEvents()
    {
        _avatarsButton.onClick.RemoveListener(ShowAvatarsTab);
    }

    private void OnDestroy()
    {
        UnSubscribeOnEvents();
    }
}