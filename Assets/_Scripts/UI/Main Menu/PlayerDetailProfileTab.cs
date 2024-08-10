using System.Collections.Generic;
using UnityEngine;

public class PlayerDetailProfileTab : TabView
{
    [SerializeField] private ProfileTabs _profileTabs;

    public AvatarsTab AvatarsTab => _profileTabs.AvatarsTab;

    public void Initialize(IReadOnlyList<AvatarData> avatarsData)
    {
        _profileTabs.Initialize(avatarsData);
    }

    public override void Show()
    {
        _profileTabs.Show();
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        _profileTabs.Hide();
    }
}