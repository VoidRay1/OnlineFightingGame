using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendsTabs : MultiTabsView
{
    [SerializeField] private Button _friendsTabButton;
    [SerializeField] private Button _incomingFriendsRequestsTabButton;
    [SerializeField] private Button _blockedPlayersTabButton;
    [SerializeField] private FriendsTab _friendsTab;
    [SerializeField] private IncomingFriendsRequestsTab _incomingFriendsRequestsTab;
    [SerializeField] private BlockedPlayersTab _blockedPlayersTab;

    private AudioClip _buttonClickedAudioClip;
    private TabView _selectedTabView;

    public event Action<PlayerData> OnAcceptFriendRequest;
    public event Action<PlayerData> OnDeclineFriendRequest;
    public event Action<PlayerData> OnRemoveFriend;
    public event Action<PlayerData> OnBlockPlayer;
    public event Action<PlayerData> OnUnBlockPlayer;

    public void Initialize()
    {
        SetTabs(new List<TabView> { _friendsTab, _incomingFriendsRequestsTab, _blockedPlayersTab });
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        _selectedTabView = _friendsTab;
        HideTabs();
    }

    public override void Show()
    {
        SubscribeOnEvents();
        ShowTabWithoutNavigationOnDown(_selectedTabView);
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        UnSubscribeOnEvents();
    }

    public void SelectActiveTabView()
    {
        _selectedTabView.ActiveButton.Select();
    }

    private void ShowFriendsTab()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _selectedTabView = _friendsTab;
        ShowTabWithoutNavigationOnDown(_friendsTab);
    }

    private void ShowIncomingFriendsRequestsTab()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _selectedTabView = _incomingFriendsRequestsTab;
        ShowTabWithoutNavigationOnDown(_incomingFriendsRequestsTab);
    }

    private void ShowBlockedPlayersTab()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        _selectedTabView = _blockedPlayersTab;
        ShowTabWithoutNavigationOnDown(_blockedPlayersTab);
    }

    public void SetFriendsRelationships(IReadOnlyList<PlayerData> friendsData)
    {
        _friendsTab.RefreshFriendsViews(_friendsTabButton, friendsData);
    }

    public void SetIncomingFriendsRequests(IReadOnlyList<PlayerData> incomingFriendsRequestsData)
    {
        _incomingFriendsRequestsTab.RefreshIncomingFriendsRequests(_incomingFriendsRequestsTabButton, incomingFriendsRequestsData);
    }

    public void SetBlockedPlayers(IReadOnlyList<PlayerData> blockedPlayersData)
    {
        _blockedPlayersTab.RefreshBlockedPlayers(_blockedPlayersTabButton, blockedPlayersData);
    }

    public void AddFriendView(PlayerData friendData)
    {
        _friendsTab.AddFriendView(friendData);
    }

    public void RemoveFriendView(PlayerData friendData)
    {
        _friendsTab.RemoveFriendView(friendData);
    }

    public void AddBlockedPlayerView(PlayerData playerData)
    {
        _blockedPlayersTab.AddBlockedPlayerView(playerData);
    }

    public void RemoveBlockedPlayerView(PlayerData blockedPlayerData)
    {
        _blockedPlayersTab.RemoveBlockedPlayerView(blockedPlayerData);
    }

    public void RemoveIncomingFriendRequestView(PlayerData incomingFriendRequestData)
    {
        _incomingFriendsRequestsTab.RemoveIncomingFriendRequestView(incomingFriendRequestData);
    }

    private void AcceptFriendRequest(PlayerData friendData)
    {
        OnAcceptFriendRequest?.Invoke(friendData);
    }

    private void DeclineFriendRequest(PlayerData friendData)
    {
        OnDeclineFriendRequest?.Invoke(friendData);
    }

    private void BlockPlayer(PlayerData playerData)
    {
        OnBlockPlayer?.Invoke(playerData);
    }

    private void RemoveFriend(PlayerData friendData)
    {
        OnRemoveFriend?.Invoke(friendData);
    }

    private void UnBlockPlayer(PlayerData playerData)
    {
        OnUnBlockPlayer?.Invoke(playerData);
    }

    private void SubscribeOnEvents()
    {
        _friendsTabButton.onClick.AddListener(ShowFriendsTab);
        _incomingFriendsRequestsTabButton.onClick.AddListener(ShowIncomingFriendsRequestsTab);
        _blockedPlayersTabButton.onClick.AddListener(ShowBlockedPlayersTab);
        _friendsTab.OnRemoveFriend += RemoveFriend;
        _friendsTab.OnBlockFriend += BlockPlayer;
        _incomingFriendsRequestsTab.OnAcceptFriendRequest += AcceptFriendRequest;
        _incomingFriendsRequestsTab.OnDeclineFriendRequest += DeclineFriendRequest;
        _incomingFriendsRequestsTab.OnBlockPlayer += BlockPlayer;
        _blockedPlayersTab.OnUnBlockPlayer += UnBlockPlayer;
    }

    private void UnSubscribeOnEvents()
    {
        _friendsTabButton.onClick.RemoveListener(ShowFriendsTab);
        _incomingFriendsRequestsTabButton.onClick.RemoveListener(ShowIncomingFriendsRequestsTab);
        _blockedPlayersTabButton.onClick.RemoveListener(ShowBlockedPlayersTab);
        _friendsTab.OnRemoveFriend -= RemoveFriend;
        _friendsTab.OnBlockFriend -= BlockPlayer;
        _incomingFriendsRequestsTab.OnAcceptFriendRequest -= AcceptFriendRequest;
        _incomingFriendsRequestsTab.OnDeclineFriendRequest -= DeclineFriendRequest;
        _incomingFriendsRequestsTab.OnBlockPlayer -= BlockPlayer;
        _blockedPlayersTab.OnUnBlockPlayer -= UnBlockPlayer;
    }

    private void OnDestroy()
    {
        UnSubscribeOnEvents();
    }
}