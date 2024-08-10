using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class FriendsTab : TabView
{
    [SerializeField] private FriendView _friendViewPrefab;
    [SerializeField] private Transform _friendViewParent;

    private readonly List<FriendView> _friendViews = new List<FriendView>();
    private Selectable _selectableOnUp;

    public event Action<PlayerData> OnRemoveFriend;
    public event Action<PlayerData> OnBlockFriend;

    public void RefreshFriendsViews(Selectable selectableOnUp, IReadOnlyList<PlayerData> friendsData)
    {
        _selectableOnUp = selectableOnUp;
        ClearFriendsViews();
        for (int i = 0; i < friendsData.Count; i++)
        {
            FriendView friendView = Instantiate(_friendViewPrefab, _friendViewParent);
            _friendViews.Add(friendView);
        }
        for (int i = 0; i < _friendViews.Count; i++)
        {
            if (i == 0)
            {
                selectableOnUp.SetSelectableOnDown(_friendViews[i].RemoveFriendButton);
            }
            SetNavigationForFriendView(_friendViews[i], i);
            _friendViews[i].Show(friendsData[i], RemoveFriend, BlockFriend);
        }
    }

    public void AddFriendView(PlayerData friendData)
    {
        FriendView friendView = Instantiate(_friendViewPrefab, _friendViewParent);
        friendView.Show(friendData, RemoveFriend, BlockFriend);
        _friendViews.Add(friendView);
        if (_friendViews.Count == 1)
        {
            _selectableOnUp.SetSelectableOnDown(_friendViews[0].RemoveFriendButton);
        }
        SetNavigationForFriendView(friendView, _friendViews.Count - 1);
    }

    public void RemoveFriendView(PlayerData friendData)
    {
        FriendView friendView = _friendViews.First(friendView => friendView.FriendData.Id == friendData.Id);
        int friendViewIndex = _friendViews.IndexOf(friendView);
        Destroy(friendView.gameObject);
        _friendViews.Remove(friendView);
        if (friendViewIndex == 0 && _friendViews.Count > 0)
        {
            _selectableOnUp.SetSelectableOnDown(_friendViews[0].RemoveFriendButton);
        }
    }

    private void SetNavigationForFriendView(FriendView friendView, int friendViewIndex)
    {
        friendView.RemoveFriendButton.SetSelectableOnUp(_selectableOnUp);
        friendView.RemoveFriendButton.SetSelectableOnRight(friendView.BlockFriendButton);
        friendView.BlockFriendButton.SetSelectableOnUp(_selectableOnUp);
        friendView.BlockFriendButton.SetSelectableOnLeft(friendView.RemoveFriendButton);
        if (friendViewIndex != 0)
        {
            friendView.RemoveFriendButton.SetSelectableOnLeft(_friendViews[friendViewIndex - 1].BlockFriendButton);
        }
        if (friendViewIndex < _friendViews.Count - 1)
        {
            friendView.BlockFriendButton.SetSelectableOnRight(_friendViews[friendViewIndex + 1].RemoveFriendButton);
        }
    }

    private void RemoveFriend(PlayerData playerData)
    {
        OnRemoveFriend?.Invoke(playerData);
    }

    private void BlockFriend(PlayerData playerData)
    {
        OnBlockFriend?.Invoke(playerData);
    }

    private void ClearFriendsViews()
    {
        foreach (FriendView friendView in _friendViews)
        {
            Destroy(friendView.gameObject);
        }
        _friendViews.Clear();
    }
}