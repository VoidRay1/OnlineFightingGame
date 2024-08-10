using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendTab : TabView
{
    [SerializeField] private FriendsTabs _friendsTabs;
    [SerializeField] private Button _refreshButton;
    [SerializeField] private Button _addFriendButton;

    private AudioClip _buttonClickedAudioClip;

    public event Action OnFriendsTabDestroyed;
    public event Action OnRefresh;
    public event Action OnAddFriend;
    public event Action<PlayerData> OnAcceptFriendRequest;
    public event Action<PlayerData> OnDeclineFriendRequest;
    public event Action<PlayerData> OnRemoveFriend;
    public event Action<PlayerData> OnBlockPlayer;
    public event Action<PlayerData> OnUnBlockPlayer;

    public void Initialize()
    {
        _friendsTabs.Initialize();
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
    }

    public override void Show()
    {
        SubscribeOnEvents();
        _friendsTabs.Show();
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        _friendsTabs.Hide();
        UnSubscribeOnEvents();
    }

    public void SelectActiveTabView()
    {
        _friendsTabs.SelectActiveTabView();
    }

    public void SetFriendsRelationships(IReadOnlyList<PlayerData> friendsData)
    {
        _friendsTabs.SetFriendsRelationships(friendsData);
    }

    public void SetIncomingFriendsRequests(IReadOnlyList<PlayerData> incomingFriendsRequestsData)
    {
        _friendsTabs.SetIncomingFriendsRequests(incomingFriendsRequestsData);
    }

    public void SetBlockedPlayers(IReadOnlyList<PlayerData> blockedPlayersData)
    {
        _friendsTabs.SetBlockedPlayers(blockedPlayersData);
    }

    public void AddFriendView(PlayerData friendData)
    {
        _friendsTabs.AddFriendView(friendData);
    }

    public void RemoveFriendView(PlayerData friendData)
    {
        _friendsTabs.RemoveFriendView(friendData);
    }

    public void AddBlockedPlayerView(PlayerData playerData)
    {
        _friendsTabs.AddBlockedPlayerView(playerData);
    }

    public void RemoveBlockedPlayerView(PlayerData blockedPlayerData)
    {
        _friendsTabs.RemoveBlockedPlayerView(blockedPlayerData);
    }

    public void RemoveIncomingFriendRequestView(PlayerData incomingFriendRequestData)
    {
        _friendsTabs.RemoveIncomingFriendRequestView(incomingFriendRequestData);
    }

    private void Refresh()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        OnRefresh?.Invoke();
    }

    private void AddFriend()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        OnAddFriend?.Invoke();
    }

    private void AcceptFriendRequest(PlayerData friendData)
    {
        OnAcceptFriendRequest?.Invoke(friendData);
    }

    private void DeclineFriendRequest(PlayerData friendData)
    {
        OnDeclineFriendRequest?.Invoke(friendData);
    }

    private void RemoveFriend(PlayerData friendData)
    {
        OnRemoveFriend?.Invoke(friendData);
    }

    private void BlockPlayer(PlayerData playerData)
    {
        OnBlockPlayer?.Invoke(playerData);
    }

    private void UnBlockPlayer(PlayerData playerData)
    {
        OnUnBlockPlayer?.Invoke(playerData);
    }

    private void SubscribeOnEvents()
    {
        _refreshButton.onClick.AddListener(Refresh);
        _addFriendButton.onClick.AddListener(AddFriend);
        _friendsTabs.OnAcceptFriendRequest += AcceptFriendRequest;
        _friendsTabs.OnDeclineFriendRequest += DeclineFriendRequest;
        _friendsTabs.OnRemoveFriend += RemoveFriend;
        _friendsTabs.OnBlockPlayer += BlockPlayer;
        _friendsTabs.OnUnBlockPlayer += UnBlockPlayer;
    }

    private void UnSubscribeOnEvents()
    {
        _refreshButton.onClick.RemoveListener(Refresh);
        _addFriendButton.onClick.RemoveListener(AddFriend);
        _friendsTabs.OnAcceptFriendRequest -= AcceptFriendRequest;
        _friendsTabs.OnDeclineFriendRequest -= DeclineFriendRequest;
        _friendsTabs.OnRemoveFriend -= RemoveFriend;
        _friendsTabs.OnBlockPlayer -= BlockPlayer;
        _friendsTabs.OnUnBlockPlayer -= UnBlockPlayer;
    }

    private void OnDestroy()
    {
        UnSubscribeOnEvents();
        OnFriendsTabDestroyed?.Invoke();
    }
}