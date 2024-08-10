using System;
using UnityEngine;

public class MemberOfGroupProfileView : BasePlayerProfileView
{
    [SerializeField] private HoverHintButton _kickMemberFromGroupButton;
    [SerializeField] private HoverHintButton _makeMemeberLeaderOfGroupButton;

    public event Action<PlayerData> OnKickMemberFromGroup;
    public event Action<PlayerData> OnMakeMemberLeaderOfGroup;

    private void Awake()
    {
        HideLeaderButtonsForMember();
    }

    public override void Initialize(PlayerData playerData)
    {
        base.Initialize(playerData);
    }

    public override void Show()
    {
        _kickMemberFromGroupButton.Button.onClick.AddListener(KickMemberFromGroup);
        _makeMemeberLeaderOfGroupButton.Button.onClick.AddListener(MakeMemberLeaderOfGroup);
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        UnSubscribeOnEvents();
    }

    public void ShowButtonsForLeader()
    {
        ShowKickMemberFromGroupButton();
        ShowMakeMemberLeaderOfGroupButton();
    }

    public void HideLeaderButtonsForMember()
    {
        HideKickMemberFromGroupButton();
        HideMakeMemberLeaderOfGroupButton();
    }

    public void ShowKickMemberFromGroupButton()
    {
        _kickMemberFromGroupButton.gameObject.SetActive(true);
    }

    public void HideKickMemberFromGroupButton()
    {
        _kickMemberFromGroupButton.gameObject.SetActive(false);
        _kickMemberFromGroupButton.TryUnloadHoverHint();
    }

    public void ShowMakeMemberLeaderOfGroupButton()
    {
        _makeMemeberLeaderOfGroupButton.gameObject.SetActive(true);
    }

    public void HideMakeMemberLeaderOfGroupButton()
    {
        _makeMemeberLeaderOfGroupButton.gameObject.SetActive(false);
        _makeMemeberLeaderOfGroupButton.TryUnloadHoverHint();
    }

    private void KickMemberFromGroup()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(ButtonClickedAudioClip);
        OnKickMemberFromGroup?.Invoke(PlayerData);
    }

    private void MakeMemberLeaderOfGroup()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(ButtonClickedAudioClip);
        OnMakeMemberLeaderOfGroup?.Invoke(PlayerData);
    }

    private void OnDestroy()
    {
        UnSubscribeOnEvents();
    }

    private void UnSubscribeOnEvents()
    {
        _kickMemberFromGroupButton.Button.onClick.RemoveListener(KickMemberFromGroup);
        _makeMemeberLeaderOfGroupButton.Button.onClick.RemoveListener(MakeMemberLeaderOfGroup);
    }
}