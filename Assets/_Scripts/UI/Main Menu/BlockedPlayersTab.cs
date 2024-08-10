using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class BlockedPlayersTab : TabView
{
    [SerializeField] private BlockedPlayerView _blockedPlayerViewPrefab;
    [SerializeField] private Transform _blockedPlayerViewParent;

    private readonly List<BlockedPlayerView> _blockedPlayerViews = new List<BlockedPlayerView>();
    private Selectable _selectableOnUp;

    public event Action<PlayerData> OnUnBlockPlayer;

    public void RefreshBlockedPlayers(Selectable selectableOnUp, IReadOnlyList<PlayerData> blockedPlayersData)
    {
        _selectableOnUp = selectableOnUp;
        ClearBlockedPlayers();
        for (int i = 0; i < blockedPlayersData.Count; i++)
        {
            BlockedPlayerView blockedPlayerView = Instantiate(_blockedPlayerViewPrefab, _blockedPlayerViewParent);
            _blockedPlayerViews.Add(blockedPlayerView);
        }
        for (int i = 0; i < _blockedPlayerViews.Count; i++)
        {
            if (i == 0)
            {
                selectableOnUp.SetSelectableOnDown(_blockedPlayerViews[i].UnBlockPlayerButton);
            }
            SetNavigationForBlockedPlayerView(_blockedPlayerViews[i], i);
            _blockedPlayerViews[i].Show(blockedPlayersData[i], UnBlockPlayer);
        }
    }

    private void SetNavigationForBlockedPlayerView(BlockedPlayerView blockedPlayerView, int blockedPlayerViewIndex)
    {
        blockedPlayerView.UnBlockPlayerButton.SetSelectableOnUp(_selectableOnUp);
        if (blockedPlayerViewIndex != 0)
        {
            blockedPlayerView.UnBlockPlayerButton.SetSelectableOnLeft(_blockedPlayerViews[blockedPlayerViewIndex - 1].UnBlockPlayerButton);
        }
        if (blockedPlayerViewIndex < _blockedPlayerViews.Count - 1)
        {
            blockedPlayerView.UnBlockPlayerButton.SetSelectableOnRight(_blockedPlayerViews[blockedPlayerViewIndex + 1].UnBlockPlayerButton);
        }
    }

    public void AddBlockedPlayerView(PlayerData playerData)
    {
        BlockedPlayerView blockedPlayerView = Instantiate(_blockedPlayerViewPrefab, _blockedPlayerViewParent);
        blockedPlayerView.Show(playerData, UnBlockPlayer);
        _blockedPlayerViews.Add(blockedPlayerView);
        if (_blockedPlayerViews.Count == 1)
        {
            _selectableOnUp.SetSelectableOnDown(_blockedPlayerViews[0].UnBlockPlayerButton);
        }
        SetNavigationForBlockedPlayerView(blockedPlayerView, _blockedPlayerViews.Count - 1);
    }

    public void RemoveBlockedPlayerView(PlayerData playerData)
    {
        BlockedPlayerView blockedPlayerView = _blockedPlayerViews.First(blockedPlayerView => blockedPlayerView.BlockedPlayerData.Id == playerData.Id);
        int blockedPlayerViewIndex = _blockedPlayerViews.IndexOf(blockedPlayerView);
        Destroy(blockedPlayerView.gameObject);
        _blockedPlayerViews.Remove(blockedPlayerView);
        if (blockedPlayerViewIndex == 0 && _blockedPlayerViews.Count > 0)
        {
            _selectableOnUp.SetSelectableOnDown(_blockedPlayerViews[0].UnBlockPlayerButton);
        }
    }

    private void UnBlockPlayer(PlayerData playerData)
    {
        OnUnBlockPlayer?.Invoke(playerData);
    }

    private void ClearBlockedPlayers()
    {
        foreach (BlockedPlayerView blockedPlayerView in _blockedPlayerViews)
        {
            Destroy(blockedPlayerView.gameObject);
        }
        _blockedPlayerViews.Clear();
    }
}