using System.Collections.Generic;

public class SettingsMenuProvider : AddressableLoader
{
    private readonly List<string> _qualityPresets = new List<string>();
    private readonly List<string> _resolutions = new List<string>();
    private readonly List<string> _languages = new List<string>();
    private SettingsMenuTabs _settingsMenu;

    private SettingsData SettingsData => GameContext.Instance.SettingsData;

    public SettingsMenuProvider(List<string> qualityPresets, List<string> resolutions, List<string> languages)
    {
        _qualityPresets = qualityPresets;
        _resolutions = resolutions;
        _languages = languages;
    }

    public async void ShowQualitySettingsTab(BaseView windowToShow)
    {
        _settingsMenu = await Load<SettingsMenuTabs>(Constants.Addressables.Keys.SettingsMenu);
        _settingsMenu.Initialize(_resolutions, _qualityPresets, _languages, SettingsData);
        _settingsMenu.OnViewHidden += Unload;
        GameContext.Instance.WindowCloser.AddPairWindow(_settingsMenu, windowToShow);
        _settingsMenu.ShowQualitySettingsTab();
    }

    private void Unload()
    {
        _settingsMenu.OnViewHidden -= Unload;
        _settingsMenu = null;
        UnloadCachedGameObject();
    }
}