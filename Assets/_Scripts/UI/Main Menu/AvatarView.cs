using System;
using UnityEngine;
using UnityEngine.UI;

public class AvatarView : BaseView
{
    [SerializeField] private RawImage _avatar;
    [SerializeField] private Button _setAvatarButton;
    [SerializeField] private Button _deleteAvatarButton;

    private AvatarData _avatarData;
    private AudioClip _buttonClickedAudioClip;
    private event Action<AvatarData> _onSetAvatar;
    private event Action<AvatarData> _onDeleteAvatar;

    public Button SetAvatarButton => _setAvatarButton;
    public Button DeleteAvatarButton => _deleteAvatarButton;

    public AvatarData AvatarData => _avatarData;

    public void Show(AvatarData avatarData, Action<AvatarData> onSetAvatar, Action<AvatarData> onDeleteAvatar)
    {
        _avatarData = avatarData;
        _avatar.texture = avatarData.Texture;
        _onSetAvatar = onSetAvatar;
        _onDeleteAvatar = onDeleteAvatar;
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        _setAvatarButton.onClick.AddListener(SetAvatar);
        _deleteAvatarButton.onClick.AddListener(DeleteAvatar);
        base.Show();
    }

    private void SetAvatar()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _onSetAvatar?.Invoke(_avatarData);
    }

    private void DeleteAvatar()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _onDeleteAvatar?.Invoke(_avatarData);
    }

    private void OnDestroy()
    {
        _setAvatarButton.onClick.RemoveListener(SetAvatar);
        _deleteAvatarButton.onClick.RemoveListener(DeleteAvatar);
    }
}