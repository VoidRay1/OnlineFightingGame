using System.Collections.Generic;
using System.Linq;
using Unity.Services.Friends;
using Unity.Services.Friends.Models;
using Unity.Services.Samples.Friends;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class MainMenuSceneEntryPoint : MonoBehaviour
{
    [SerializeField] private UnityWebRequestService _webRequestService;
    [SerializeField] private Matchmaker _matchmaker;
    [SerializeField] private GlobalMainMenuView _globalMainMenuView;
    [SerializeField] private Selectable _firstMainMenuSelectedTab;

    private Activity _activity = new Activity { Status = "In Main Menu" };
    private SettingsMenuProvider _settingsMenuProvider;

    private async void Start()
    {
#if SERVER == false
        _globalMainMenuView.Hide();
        await FriendsService.Instance.SetPresenceAsync(Availability.Online, _activity);
        _settingsMenuProvider = new SettingsMenuProvider(GetQualityPresets(), GetResolutions(), GetLanguages());
        var groupCreator = new GroupCreator(_globalMainMenuView);
        var gameCloser = new GameCloser(groupCreator);
        GameContext.Instance.PlayerData.Activity = _activity.Status;
        GameContext.Instance.PopupMessageProvider.SetParent(_globalMainMenuView.Canvas.transform);
        _globalMainMenuView.Initialize(_firstMainMenuSelectedTab, GameContext.Instance.PlayerData, gameCloser);
        _matchmaker.Initialize(groupCreator);
        new MainMenu(_globalMainMenuView, _settingsMenuProvider, _webRequestService);
        _globalMainMenuView.Show();
#endif
    }

    private List<string> GetQualityPresets()
    {
        return QualitySettings.names.ToList();
    }

    private List<string> GetResolutions()
    {
        List<string> resolutions = new List<string>();
        foreach (Resolution resolution in GameContext.Instance.Resolutions)
        {
            resolutions.Add($"{resolution.width}x{resolution.height}");
        }
        return resolutions;
    }

    private List<string> GetLanguages()
    {
        List<string> locales = new List<string>();
        foreach (Locale locale in LocalizationSettings.AvailableLocales.Locales)
        {
            locales.Add(locale.name);
        }
        return locales;
    }
}