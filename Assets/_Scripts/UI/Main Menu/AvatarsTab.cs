using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AvatarsTab : TabView
{
    [SerializeField] private AvatarView _avatarViewPrefab;
    [SerializeField] private Transform _avatarViewParent;

    private readonly List<AvatarView> _avatarViews = new List<AvatarView>();
    private Selectable _selectableOnUp;

    public event Action OnAvatarsTabDestroyed;
    public event Action<AvatarData> OnSetAvatar;
    public event Action<AvatarData> OnDeleteAvatar;

    public void RefreshAvatarsViews(Selectable selectableOnUp, IReadOnlyList<AvatarData> avatarsData)
    {
        _selectableOnUp = selectableOnUp;
        ClearAvatarsViews();
        for (int i = 0; i < avatarsData.Count; i++)
        {
            AvatarView avatarView = Instantiate(_avatarViewPrefab, _avatarViewParent);
            _avatarViews.Add(avatarView);
        }
        for (int i = 0; i < _avatarViews.Count; i++)
        {
            if (i == 0)
            {
                selectableOnUp.SetSelectableOnDown(_avatarViews[i].SetAvatarButton);
            }
            SetNavigationForAvatarView(_avatarViews[i], i);
            _avatarViews[i].Show(avatarsData[i], SetAvatar, DeleteAvatar);
        }
    }

    public void RemoveAvatar(AvatarData avatarData)
    {
        AvatarView avatarView = _avatarViews.First(avatarView => avatarView.AvatarData.Id == avatarData.Id);
        int avatarViewIndex = _avatarViews.IndexOf(avatarView);
        Destroy(avatarView.gameObject);
        _avatarViews.Remove(avatarView);
        if (avatarViewIndex == 0 && _avatarViews.Count > 0)
        {
            _selectableOnUp.SetSelectableOnDown(_avatarViews[0].SetAvatarButton);
        }
    }

    private void SetAvatar(AvatarData avatarData)
    {
        OnSetAvatar?.Invoke(avatarData);
    }

    private void DeleteAvatar(AvatarData avatarData)
    {
        OnDeleteAvatar?.Invoke(avatarData);
    }

    private void SetNavigationForAvatarView(AvatarView avatarView, int avatarViewIndex)
    {
        avatarView.SetAvatarButton.SetSelectableOnUp(_selectableOnUp);
        avatarView.SetAvatarButton.SetSelectableOnRight(avatarView.DeleteAvatarButton);
        avatarView.DeleteAvatarButton.SetSelectableOnUp(_selectableOnUp);
        avatarView.DeleteAvatarButton.SetSelectableOnLeft(avatarView.SetAvatarButton);
        if (avatarViewIndex != 0)
        {
            avatarView.SetAvatarButton.SetSelectableOnLeft(_avatarViews[avatarViewIndex - 1].DeleteAvatarButton);
        }
        if (avatarViewIndex < _avatarViews.Count - 1)
        {
            avatarView.DeleteAvatarButton.SetSelectableOnRight(_avatarViews[avatarViewIndex + 1].SetAvatarButton);
        }
    }

    private void ClearAvatarsViews()
    {
        foreach (AvatarView avatarView in _avatarViews)
        {
            Destroy(avatarView.gameObject);
        }
        _avatarViews.Clear();
    }

    private void OnDestroy()
    {
        OnAvatarsTabDestroyed?.Invoke();
    }
}