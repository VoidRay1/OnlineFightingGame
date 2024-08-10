using System.Collections.Generic;
using System.Threading.Tasks;

public class PlayerFeaturesProvider : AddressableLoader
{
    private PlayerFeaturesTabs _playerFeaturesTabs;

    public async void ShowPlayerDetailProfileTab(FriendsServiceManager friendsServiceManager, CustomAvatarService customAvatarService,
        IReadOnlyList<AvatarData> avatarsData, BaseView windowToShow)
    {
        await LoadPlayerFeaturesTabs(friendsServiceManager, customAvatarService, avatarsData, windowToShow);
        _playerFeaturesTabs.ShowPlayerDetailProfileTab();
    }

    public async void ShowFriendsTab(FriendsServiceManager friendsServiceManager, CustomAvatarService customAvatarService,
        IReadOnlyList<AvatarData> avatarsData, BaseView windowToShow)
    {
        await LoadPlayerFeaturesTabs(friendsServiceManager, customAvatarService, avatarsData, windowToShow);
        _playerFeaturesTabs.ShowFriendsTab();
    }

    private async Task LoadPlayerFeaturesTabs(FriendsServiceManager friendsServiceManager, CustomAvatarService customAvatarService,
        IReadOnlyList<AvatarData> avatarsData, BaseView windowToShow)
    {
        _playerFeaturesTabs = await Load<PlayerFeaturesTabs>(Constants.Addressables.Keys.PlayerFeatures);
        _playerFeaturesTabs.Initialize(avatarsData);
        friendsServiceManager.SetFriendsTab(_playerFeaturesTabs.FriendsTab);
        customAvatarService.SetAvatarsTab(_playerFeaturesTabs.PlayerDetailProfileTab.AvatarsTab);
        _playerFeaturesTabs.OnViewHidden += Unload;
        GameContext.Instance.WindowCloser.AddPairWindow(_playerFeaturesTabs, windowToShow);
    }

    private void Unload()
    {
        _playerFeaturesTabs.OnViewHidden -= Unload;
        _playerFeaturesTabs = null;
        UnloadCachedGameObject();
    }
}