using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileView : BasePlayerProfileView
{
    [SerializeField] private Image _leaveGroupImage;
    [SerializeField] private Image _createGroupImage;
    [SerializeField] private Button _playerDetailProfileButton;
    [SerializeField] private HoverHintButton _uploadCustomAvatarButton;
    [SerializeField] private HoverHintButton _closeGroupButton;
    [SerializeField] private HoverHintButton _leaveGroupButton;
    [SerializeField] private HoverHintButton _createGroupButton;
    [SerializeField] private HoverHintButton _joinGroupButton;

    public event Action OnShowPlayerDetailProfile;
    public event Action OnUploadCustomAvatar;
    public event Action OnCloseGroup;
    public event Action<PlayerData> OnLeaveGroup;
    public event Action OnCreateGroup;
    public event Action OnJoinGroup;
    public event Action OnPlayerProfileDestroyed;

    public override void Initialize(PlayerData playerData)
    {
        base.Initialize(playerData);
        SubscribeOnEvents();
        HideGroupLeaderImage();
    }

    public void ShowButtonsForNoActiveGroup()
    {
        HideGroupLeaderImage();
        ShowJoinGroupButton();
        ShowCreateGroupButton();
        HideLeaveGroupButton();
        HideCloseGroupButton();
    }

    public void ShowButtonsForLeader()
    {
        HideJoinGroupButton();
        HideCreateGroupButton();
        HideLeaveGroupButton();
        ShowCloseGroupButton();
    }

    public void ShowButtonsForMember()
    {
        HideJoinGroupButton();
        HideCreateGroupButton();
        ShowLeaveGroupButton();
        HideCloseGroupButton();
    }

    public void ShowButtonsForNotFullGroup()
    {
        ShowGroupLeaderImage();
        HideJoinGroupButton();
        HideCreateGroupButton();
        HideLeaveGroupButton();
        ShowCloseGroupButton();
    }

    public void HideAllGroupButtons()
    {
        HideJoinGroupButton();
        HideCreateGroupButton();
        HideLeaveGroupButton();
        HideCloseGroupButton();
    }

    public override void Hide()
    {
        base.Hide();
        UnSubscribeOnEvents();
    }

    public void SelectJoinGroupButton()
    {
        _joinGroupButton.Button.Select();
    }

    public void HideUploadAvatarButtonHoverHint()
    {
        _uploadCustomAvatarButton.HideHoverHint();
    }

    public void ShowUploadAvatarButtonHoverHint()
    {
        _uploadCustomAvatarButton.ShowHoverHint();
    }

    public void SetSelectableOnLeft(Selectable selectableOnLeft)
    {
        _uploadCustomAvatarButton.Button.SetSelectableOnLeft(selectableOnLeft);
    }

    public void ShowCloseGroupButton()
    {
        _closeGroupButton.gameObject.SetActive(true);
    }

    public void HideCloseGroupButton()
    {
        _closeGroupButton.gameObject.SetActive(false);
        _closeGroupButton.TryUnloadHoverHint();
    }

    public void ShowLeaveGroupButton()
    {
        _leaveGroupButton.gameObject.SetActive(true);
    }

    public void HideLeaveGroupButton()
    {
        _leaveGroupButton.gameObject.SetActive(false);
        _leaveGroupButton.TryUnloadHoverHint();
    }

    public void ShowCreateGroupButton()
    {
        _createGroupButton.gameObject.SetActive(true);
    }

    public void HideCreateGroupButton()
    {
        _createGroupButton.gameObject.SetActive(false);
        _createGroupButton.TryUnloadHoverHint();
    }

    public void ShowJoinGroupButton()
    {
        _joinGroupButton.gameObject.SetActive(true);
    }

    public void HideJoinGroupButton()
    {
        _joinGroupButton.gameObject.SetActive(false);
        _joinGroupButton.TryUnloadHoverHint();
    }

    private void ShowPlayerDetailProfile()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(ButtonClickedAudioClip);
        OnShowPlayerDetailProfile?.Invoke();
    }

    private void UploadCustomAvatar()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(ButtonClickedAudioClip);
        OnUploadCustomAvatar?.Invoke();
    }

    private void CloseGroup()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(ButtonClickedAudioClip);
        OnCloseGroup?.Invoke();
    }

    private void LeaveGroup()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(ButtonClickedAudioClip);
        OnLeaveGroup?.Invoke(PlayerData);
    }

    private void CreateGroup()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(ButtonClickedAudioClip);
        OnCreateGroup?.Invoke();
    }

    private void JoinGroup()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(ButtonClickedAudioClip);
        OnJoinGroup?.Invoke();
    }

    private void OnDestroy()
    {
        UnSubscribeOnEvents();
        OnPlayerProfileDestroyed?.Invoke();
    }

    private void SubscribeOnEvents()
    {
        _playerDetailProfileButton.onClick.AddListener(ShowPlayerDetailProfile);
        _uploadCustomAvatarButton.Button.onClick.AddListener(UploadCustomAvatar);
        _closeGroupButton.Button.onClick.AddListener(CloseGroup);
        _leaveGroupButton.Button.onClick.AddListener(LeaveGroup);
        _createGroupButton.Button.onClick.AddListener(CreateGroup);
        _joinGroupButton.Button.onClick.AddListener(JoinGroup);
    }

    private void UnSubscribeOnEvents()
    {
        _playerDetailProfileButton.onClick.RemoveListener(ShowPlayerDetailProfile);
        _uploadCustomAvatarButton.Button.onClick.RemoveListener(UploadCustomAvatar);
        _closeGroupButton.Button.onClick.RemoveListener(CloseGroup);
        _leaveGroupButton.Button.onClick.RemoveListener(LeaveGroup);
        _createGroupButton.Button.onClick.RemoveListener(CreateGroup);
        _joinGroupButton.Button.onClick.RemoveListener(JoinGroup);
    }
}