using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendView : BaseView
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _activity;
    [SerializeField] private Button _removeFriendButton;
    [SerializeField] private Button _blockFriendButton;

    private PlayerData _friendData;
    private AudioClip _buttonClickedAudioClip;
    private event Action<PlayerData> _onRemoveFriend;
    private event Action<PlayerData> _onBlockPlayer;

    public Button RemoveFriendButton => _removeFriendButton;
    public Button BlockFriendButton => _blockFriendButton;
    public PlayerData FriendData => _friendData;

    public void Show(PlayerData friendData, Action<PlayerData> onRemoveFriend, Action<PlayerData> onBlockFriend)
    {
        _friendData = friendData;
        _name.text = friendData.Name;
        _activity.text = friendData.Activity;
        _onRemoveFriend = onRemoveFriend;
        _onBlockPlayer = onBlockFriend;
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        _removeFriendButton.onClick.AddListener(RemoveFriendButtonClicked);
        _blockFriendButton.onClick.AddListener(BlockFriendButtonClicked);
        base.Show();
    }

    private void RemoveFriendButtonClicked()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _onRemoveFriend?.Invoke(_friendData);
    }

    private void BlockFriendButtonClicked()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _onBlockPlayer?.Invoke(_friendData);
    }

    private void OnDestroy()
    {
        _removeFriendButton.onClick.RemoveListener(RemoveFriendButtonClicked);
        _blockFriendButton.onClick.RemoveListener(BlockFriendButtonClicked);
    }
}