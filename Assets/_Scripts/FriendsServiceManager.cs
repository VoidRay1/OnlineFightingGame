using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Friends.Models;
using Unity.Services.Samples.Friends;
using UnityEngine;
using UnityEngine.Localization;

public class FriendsServiceManager
{
    private FriendTab _friendTab;
    private PopupMessageProvider PopupMessageProvider => GameContext.Instance.PopupMessageProvider;
    private InputWindowProvider InputWindowProvider => GameContext.Instance.InputWindowProvider;

    private const string ProfileTable = Constants.Localization.TableReferences.ProfileTable;
    private const string ConfirmText = "Add";
    private const string CancelText = "Cancel";
    private const string Description = "Friend format name";

    public void SetFriendsTab(FriendTab friendTab)
    {
        _friendTab = friendTab;
        Refresh();
        SubscribeOnEvents();
    }

    private void Refresh()
    {
        _friendTab.SetFriendsRelationships(GetFriendsData());
        _friendTab.SetIncomingFriendsRequests(GetIncomingFriendsRequests());
        _friendTab.SetBlockedPlayers(GetBlockedPlayers());
    }

    private void ShowAddFriendWindow()
    {
        InputWindowProvider.ShowInputWindow(Constants.Addressables.Keys.AddFriendInputWindow,
            TryAddFriend, CloseAddFriendWindow,
            new LocalizedString
            {
                TableReference = ProfileTable,
                TableEntryReference = Description
            },
            new LocalizedString
            {
                TableReference = ProfileTable,
                TableEntryReference = ConfirmText
            },
            new LocalizedString
            {
                TableReference = Constants.Localization.TableReferences.MainMenuTable,
                TableEntryReference = CancelText
            });
    }

    private void CloseAddFriendWindow()
    {
        _friendTab.SelectActiveTabView();
        InputWindowProvider.UnloadInputWindow();
    }

    private async void AcceptFriendRequest(PlayerData playerData)
    {
        try
        {
            var friend = await FriendsService.Instance.AddFriendAsync(playerData.Id);
            _friendTab.RemoveIncomingFriendRequestView(playerData);
            playerData.Activity = GetActivityStatus(friend);
            _friendTab.AddFriendView(playerData);
            _friendTab.SelectActiveTabView();
            PopupMessageProvider.ShowNeutralMessage($"Friend {playerData.Name} request successfully accepted");
        }
        catch (FriendsServiceException)
        {
            PopupMessageProvider.ShowErrorMessage("Failed to accept friend request");
        }
    }

    private async void DeclineFriendRequest(PlayerData playerData)
    {
        try
        {
            await FriendsService.Instance.DeleteIncomingFriendRequestAsync(playerData.Id);
            _friendTab.RemoveIncomingFriendRequestView(playerData);
            _friendTab.SelectActiveTabView();
            PopupMessageProvider.ShowNeutralMessage($"Friend {playerData.Name} request successfully declined");
        }
        catch (FriendsServiceException)
        {
            PopupMessageProvider.ShowErrorMessage("Failed to decline friend request");
        }
    }

    private async void RemoveFriend(PlayerData friendData)
    {
        try
        {
            await FriendsService.Instance.DeleteFriendAsync(friendData.Id);
            _friendTab.RemoveFriendView(friendData);
            _friendTab.SelectActiveTabView();
            PopupMessageProvider.ShowNeutralMessage($"Friend {friendData.Name} successfully removed");
        }
        catch (FriendsServiceException)
        {
            PopupMessageProvider.ShowErrorMessage($"Failed to remove: {friendData.Name}");
        }
    }

    private async void BlockPlayer(PlayerData playerData)
    {
        try
        {
            await FriendsService.Instance.AddBlockAsync(playerData.Id);
            _friendTab.AddBlockedPlayerView(playerData);
            _friendTab.SelectActiveTabView();
            RemoveFriend(playerData);
        }
        catch (FriendsServiceException)
        {
            PopupMessageProvider.ShowErrorMessage($"Failed to block {playerData.Name}");
        }
    }

    private async void UnBlockPlayer(PlayerData playerData)
    {
        try
        {
            await FriendsService.Instance.DeleteBlockAsync(playerData.Id);
            _friendTab.RemoveBlockedPlayerView(playerData);
            _friendTab.SelectActiveTabView();
            PopupMessageProvider.ShowNeutralMessage($"Player {playerData.Name} successfully unblocked");
        }
        catch (FriendsServiceException)
        {
            PopupMessageProvider.ShowErrorMessage($"Failed to unblock {playerData.Name}");
        }
    }

    private void FriendsTabDestroyed()
    {
        UnSubscribeOnEvents();
    }

    private async void TryAddFriend(string friendName)
    {
        var relationship = await SendFriendRequest(friendName);
        if (relationship == null)
        {
            return;
        }
        switch (relationship.Type)
        {
            case RelationshipType.Friend:
                PopupMessageProvider.ShowNeutralMessage("User is already friend");
                break;
            case RelationshipType.Block:
                PopupMessageProvider.ShowNeutralMessage("User is already blocked");
                break;
            case RelationshipType.FriendRequest:
                PopupMessageProvider.ShowNeutralMessage($"Request sended to {friendName}");
                InputWindowProvider.UnloadInputWindow();
                _friendTab.SelectActiveTabView();
                break;
        }
    }

    private async Task<Relationship> SendFriendRequest(string friendName)
    {
        try
        {
            var relationship = await FriendsService.Instance.AddFriendByNameAsync(friendName);
            return relationship;
        }
        catch (FriendsServiceException e)
        {
            if (e.ErrorCode == FriendsErrorCode.Unknown)
            {
                PopupMessageProvider.ShowErrorMessage($"No user with nickname {friendName} was found");
            }
            else
            {
                PopupMessageProvider.ShowErrorMessage($"Unknown error");
            }
            return null;
        }
    }

    private IReadOnlyList<PlayerData> GetFriendsData()
    {
        var friends = FriendsService.Instance.Friends.Except(FriendsService.Instance.Blocks).ToList();
        List<PlayerData> friendsData = new List<PlayerData>(friends.Count);
        foreach (var friend in friends)
        {
            PlayerData friendData = new PlayerData(friend.Member.Id, friend.Member.Profile.Name, friend.Member.Presence.Availability, GetActivityStatus(friend));
            friendsData.Add(friendData);
        }
        Debug.Log("Friends count: " + FriendsService.Instance.Friends.Count);
        return friendsData;
    }

    private IReadOnlyList<PlayerData> GetIncomingFriendsRequests()
    {
        var incomingFriendsRequests = FriendsService.Instance.IncomingFriendRequests;
        List<PlayerData> incomingFriendsRequestsData = new List<PlayerData>(incomingFriendsRequests.Count);
        foreach (var incomingFriendRequest in incomingFriendsRequests)
        {
            PlayerData incomingFriendRequestData = new PlayerData(incomingFriendRequest.Member.Id, incomingFriendRequest.Member.Profile.Name);
            incomingFriendsRequestsData.Add(incomingFriendRequestData);
        }
        Debug.Log("Incoming friends requests: " + FriendsService.Instance.IncomingFriendRequests.Count);
        return incomingFriendsRequestsData;
    }

    private IReadOnlyList<PlayerData> GetBlockedPlayers()
    {
        var blockedPlayers = FriendsService.Instance.Blocks;
        List<PlayerData> blockedPlayersData = new List<PlayerData>(blockedPlayers.Count);
        foreach (var blockedPlayer in blockedPlayers)
        {
            PlayerData blockedPlayerData = new PlayerData(blockedPlayer.Member.Id, blockedPlayer.Member.Profile.Name);
            blockedPlayersData.Add(blockedPlayerData);
        }
        Debug.Log("Blocked players count: " + FriendsService.Instance.Blocks.Count);
        return blockedPlayersData;
    }

    private string GetActivityStatus(Relationship friend)
    {
        if (friend.Member.Presence.Availability == Availability.Invisible
            || friend.Member.Presence.Availability == Availability.Offline)
        {
            DateTime lastSeenTime = friend.Member.Presence.LastSeen;
            DateTime currentTime = DateTime.UtcNow;
            TimeSpan timeDifference = currentTime - lastSeenTime;
            return timeDifference.GetTelegramLastSeenStringFormat();
        }
        return friend.Member.Presence.GetActivity<Activity>().Status;
    }

    private void SubscribeOnEvents()
    {
        _friendTab.OnRefresh += Refresh;
        _friendTab.OnAddFriend += ShowAddFriendWindow;
        _friendTab.OnFriendsTabDestroyed += FriendsTabDestroyed;
        _friendTab.OnAcceptFriendRequest += AcceptFriendRequest;
        _friendTab.OnDeclineFriendRequest += DeclineFriendRequest;
        _friendTab.OnRemoveFriend += RemoveFriend;
        _friendTab.OnBlockPlayer += BlockPlayer;
        _friendTab.OnUnBlockPlayer += UnBlockPlayer;
    }

    private void UnSubscribeOnEvents()
    {
        _friendTab.OnRefresh -= Refresh;
        _friendTab.OnAddFriend -= ShowAddFriendWindow;
        _friendTab.OnFriendsTabDestroyed -= FriendsTabDestroyed;
        _friendTab.OnAcceptFriendRequest -= AcceptFriendRequest;
        _friendTab.OnDeclineFriendRequest -= DeclineFriendRequest;
        _friendTab.OnRemoveFriend -= RemoveFriend;
        _friendTab.OnBlockPlayer -= BlockPlayer;
        _friendTab.OnUnBlockPlayer -= UnBlockPlayer;
    }
}