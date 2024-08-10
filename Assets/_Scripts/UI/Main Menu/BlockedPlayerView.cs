using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlockedPlayerView : BaseView
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private Button _unBlockPlayerButton;

    private PlayerData _blockedPlayerData;
    private AudioClip _buttonClickedAudioClip;
    private event Action<PlayerData> _onUnBlockPlayer;

    public Button UnBlockPlayerButton => _unBlockPlayerButton;

    public PlayerData BlockedPlayerData => _blockedPlayerData;

    public void Show(PlayerData blockedPlayerData, Action<PlayerData> onUnBlockFriend)
    {
        _blockedPlayerData = blockedPlayerData;
        _name.text = blockedPlayerData.Name;
        _onUnBlockPlayer = onUnBlockFriend;
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        _unBlockPlayerButton.onClick.AddListener(UnBlockFriendButtonClicked);
        base.Show();
    }

    private void UnBlockFriendButtonClicked()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _onUnBlockPlayer?.Invoke(_blockedPlayerData);
    }

    private void OnDestroy()
    {
        _unBlockPlayerButton.onClick.RemoveListener(UnBlockFriendButtonClicked);
    }
}