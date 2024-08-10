using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class IncomingFriendsRequestsTab : TabView
{
    [SerializeField] private IncomingFriendRequestView _incomingFriendRequestViewPrefab;
    [SerializeField] private Transform _incomingFriendRequestViewParent;

    private readonly List<IncomingFriendRequestView> _incomingFriendRequestViews = new List<IncomingFriendRequestView>();
    private Selectable _selectableOnUp;

    public event Action<PlayerData> OnAcceptFriendRequest;
    public event Action<PlayerData> OnDeclineFriendRequest;
    public event Action<PlayerData> OnBlockPlayer;

    public void RefreshIncomingFriendsRequests(Selectable selectableOnUp, IReadOnlyList<PlayerData> incomingFriendsRequestsData)
    {
        _selectableOnUp = selectableOnUp;
        ClearIncomingFriendsRequests();
        for (int i = 0; i < incomingFriendsRequestsData.Count; i++)
        {
            IncomingFriendRequestView incomingFriendRequestView = Instantiate(_incomingFriendRequestViewPrefab, _incomingFriendRequestViewParent);
            _incomingFriendRequestViews.Add(incomingFriendRequestView);
        }
        for (int i = 0; i < _incomingFriendRequestViews.Count; i++)
        {
            if (i == 0)
            {
                selectableOnUp.SetSelectableOnDown(_incomingFriendRequestViews[i].AcceptFriendRequestButton);
            }
            SetNavigationForIncomingFriendsRequestsView(_incomingFriendRequestViews[i], i);
            _incomingFriendRequestViews[i].Show(incomingFriendsRequestsData[i], AcceptFriendRequest, DeclineFriendRequest, BlockPlayer);
        }
    }

    public void RemoveIncomingFriendRequestView(PlayerData incomingFriendRequestData)
    {
        IncomingFriendRequestView incomingFriendRequestView = _incomingFriendRequestViews
            .First(incomingFriendRequestView => incomingFriendRequestData.Id == incomingFriendRequestData.Id);
        int incomingFriendRequestViewIndex = _incomingFriendRequestViews.IndexOf(incomingFriendRequestView);
        Destroy(incomingFriendRequestView.gameObject);
        _incomingFriendRequestViews.Remove(incomingFriendRequestView);
        if (incomingFriendRequestViewIndex == 0 && _incomingFriendRequestViews.Count > 0)
        {
            _selectableOnUp.SetSelectableOnDown(_incomingFriendRequestViews[0].AcceptFriendRequestButton);
        }
    }

    private void SetNavigationForIncomingFriendsRequestsView(IncomingFriendRequestView incomingFriendRequestView, int incomingFriendRequestViewIndex)
    {
        incomingFriendRequestView.AcceptFriendRequestButton.SetSelectableOnUp(_selectableOnUp);
        incomingFriendRequestView.AcceptFriendRequestButton.SetSelectableOnRight(incomingFriendRequestView.DeclineFriendRequestButton);
        incomingFriendRequestView.DeclineFriendRequestButton.SetSelectableOnUp(_selectableOnUp);
        incomingFriendRequestView.DeclineFriendRequestButton.SetSelectableOnLeft(incomingFriendRequestView.AcceptFriendRequestButton);
        incomingFriendRequestView.DeclineFriendRequestButton.SetSelectableOnRight(incomingFriendRequestView.BlockPlayerButton);
        incomingFriendRequestView.BlockPlayerButton.SetSelectableOnUp(_selectableOnUp);
        incomingFriendRequestView.BlockPlayerButton.SetSelectableOnLeft(incomingFriendRequestView.DeclineFriendRequestButton);
        if (incomingFriendRequestViewIndex != 0)
        {
            incomingFriendRequestView.AcceptFriendRequestButton.SetSelectableOnLeft(_incomingFriendRequestViews[incomingFriendRequestViewIndex - 1].BlockPlayerButton);
        }
        if (incomingFriendRequestViewIndex < _incomingFriendRequestViews.Count - 1)
        {
            incomingFriendRequestView.BlockPlayerButton.SetSelectableOnRight(_incomingFriendRequestViews[incomingFriendRequestViewIndex + 1].AcceptFriendRequestButton);
        }
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

    private void ClearIncomingFriendsRequests()
    {
        foreach (IncomingFriendRequestView incomingFriendRequestView in _incomingFriendRequestViews)
        {
            Destroy(incomingFriendRequestView.gameObject);
        }
        _incomingFriendRequestViews.Clear();
    }
}