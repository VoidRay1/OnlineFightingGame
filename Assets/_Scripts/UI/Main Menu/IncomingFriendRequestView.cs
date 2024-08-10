using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IncomingFriendRequestView : BaseView
{
    [SerializeField] private TMP_Text _playerName;
    [SerializeField] private Button _acceptFriendRequestButton;
    [SerializeField] private Button _declineFriendRequestButton;
    [SerializeField] private Button _blockPlayerButton;

    private PlayerData _incomingFriendRequestData;
    private AudioClip _buttonClickedAudioClip;
    private event Action<PlayerData> _onAcceptFriendRequest;
    private event Action<PlayerData> _onDeclineFriendRequest;
    private event Action<PlayerData> _onBlockPlayer;

    public Button AcceptFriendRequestButton => _acceptFriendRequestButton;
    public Button DeclineFriendRequestButton => _declineFriendRequestButton;
    public Button BlockPlayerButton => _blockPlayerButton;

    public PlayerData IncomingFriendRequestData => _incomingFriendRequestData;

    public void Show(PlayerData incomingFriendRequestData,
        Action<PlayerData> onAcceptFriendRequest, Action<PlayerData> onDeclineFriendRequest, Action<PlayerData> onBlockPlayer)
    {
        _incomingFriendRequestData = incomingFriendRequestData;
        _playerName.text = incomingFriendRequestData.Name;
        _onAcceptFriendRequest = onAcceptFriendRequest;
        _onDeclineFriendRequest = onDeclineFriendRequest;
        _onBlockPlayer = onBlockPlayer;
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        _acceptFriendRequestButton.onClick.AddListener(AcceptFriendRequestButtonClicked);
        _declineFriendRequestButton.onClick.AddListener(DeclineFriendRequestButtonClicked);
        _blockPlayerButton.onClick.AddListener(BlockPlayerButtonClicked);
        base.Show();
    }

    private void AcceptFriendRequestButtonClicked()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _onAcceptFriendRequest?.Invoke(_incomingFriendRequestData);
    }

    private void DeclineFriendRequestButtonClicked()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _onDeclineFriendRequest?.Invoke(_incomingFriendRequestData);
    }

    private void BlockPlayerButtonClicked()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _onBlockPlayer?.Invoke(_incomingFriendRequestData);
    }

    private void OnDestroy()
    {
        _acceptFriendRequestButton.onClick.RemoveListener(AcceptFriendRequestButtonClicked);
        _declineFriendRequestButton.onClick.RemoveListener(DeclineFriendRequestButtonClicked);
        _blockPlayerButton.onClick.RemoveListener(BlockPlayerButtonClicked);
    }
}