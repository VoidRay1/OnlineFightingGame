public class MainMenu
{
    private readonly GlobalMainMenuView _globalMainMenuView;
    private readonly SettingsMenuProvider _settingsMenuProvider;
    private readonly PlayerFeaturesProvider _playerFeaturesProvider;
    private readonly FriendsServiceManager _friendsServiceManager;
    private readonly CustomAvatarService _customAvatarService;
    
    public MainMenu(GlobalMainMenuView globalMainMenuView, SettingsMenuProvider settingsMenuProvider, UnityWebRequestService webRequestService)
    {
        _globalMainMenuView = globalMainMenuView;
        _settingsMenuProvider = settingsMenuProvider;
        _playerFeaturesProvider = new PlayerFeaturesProvider();
        _friendsServiceManager = new FriendsServiceManager();
        _customAvatarService = new CustomAvatarService(_globalMainMenuView.PlayerProfileView, webRequestService);
        SubscribeOnEvents();
    }

    private void ShowPlayerDetailProfile()
    {
        _globalMainMenuView.Hide();
        _playerFeaturesProvider.ShowPlayerDetailProfileTab(_friendsServiceManager, _customAvatarService, GameContext.Instance.AvatarsDataList.AvatarsData, _globalMainMenuView);
    }

    private void ShowSettingsMenu()
    {
        _globalMainMenuView.Hide();
        _settingsMenuProvider.ShowQualitySettingsTab(_globalMainMenuView);
    }

    private void SubscribeOnEvents()
    {
        _globalMainMenuView.OnShowSettingsMenu += ShowSettingsMenu;
        _globalMainMenuView.OnShowPlayerDetailProfile += ShowPlayerDetailProfile;
    }

    private void UnSubscribeOnEvents()
    {
        _globalMainMenuView.OnShowSettingsMenu -= ShowSettingsMenu;
        _globalMainMenuView.OnShowPlayerDetailProfile -= ShowPlayerDetailProfile;
    }
}