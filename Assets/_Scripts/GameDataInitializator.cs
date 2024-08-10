using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.SceneManagement;

public class GameDataInitializator : IInitializator
{
    private ISaveService SaveService => GameContext.Instance.SaveService;
    private InputWindowProvider InputWindowProvider => GameContext.Instance.InputWindowProvider;

    private readonly AvatarsDataList AvatarsDataList = new AvatarsDataList 
    { 
        AvatarsData = new List<AvatarData>() 
    };
    private const string ProfileTable = Constants.Localization.TableReferences.ProfileTable;
    private const string Description = "Enter your name";
    private SettingsData _settingsData;

    public async Task Initialize()
    {
        GameContext.Instance.Initialize();
        _settingsData = new SettingsData
        {
            Framerate = 60,
            QualityPresetIndex = (byte)(QualitySettings.names.Length - 1),
            IsFullScreenMode = true,
            IsVsyncEnabled = false,
            Volume = 0.9f,
        };
        InitializeSettingsData();
#if SERVER == false
        await InitializePlayerData();
#endif
    }

    private void InitializeSettingsData()
    {
#if SERVER == false
        GameContext.Instance.Resolutions = GetResolutions();
#endif
        if (File.Exists(Path.Combine(Application.dataPath, Constants.SettingsFileName)) == false)
        {
            _settingsData.ResolutionIndex = (byte)(GameContext.Instance.Resolutions.Count - 1);
            _settingsData.LanguageIndex = GetSystemLanguageIndex();
            SaveService.Save(Constants.SettingsFileName, _settingsData);
        }
        var settingsData = SaveService.Load<SettingsData>(Constants.SettingsFileName);
        GameContext.Instance.SetSettingsData(settingsData);
        GameContext.Instance.SettingsApplier.ApplyAllSettings();
    }

    private async Task InitializePlayerData()
    {
        if (File.Exists(Path.Combine(Application.dataPath, Constants.AvatarsDataFileName)) == false)
        {
            SaveService.Save(Constants.AvatarsDataFileName, AvatarsDataList);
        }
        if (AuthenticationService.Instance.PlayerName == null)
        {
            InputWindowProvider.ShowInputWindow(Constants.Addressables.Keys.PlayerNameInputWindow, PlayerNameSubmited, null,
                new LocalizedString
                {
                    TableReference = ProfileTable,
                    TableEntryReference = Description
                },
                new LocalizedString 
                {
                    TableReference = ProfileTable,
                    TableEntryReference = Constants.Localization.TableEntryReferences.Confirm
                },
                new LocalizedString
                {

                });
        }
        else
        {
            await LoadPlayerData();
        }
    }

    private async void PlayerNameSubmited(string playerName)
    {
        InputWindowProvider.UnloadInputWindow();
        playerName = await AuthenticationService.Instance.UpdatePlayerNameAsync(playerName);
        await CloudSaveService.Instance.Data.Player.SaveAsync(new Dictionary<string, object>
        {
            [Constants.CloudSave.Keys.PlayerId] = AuthenticationService.Instance.PlayerId,
            [Constants.CloudSave.Keys.PlayerName] = playerName,
            [Constants.CloudSave.Keys.PlayerAvatarId] = ""
        });
        await LoadPlayerData();
    }

    private async Task LoadPlayerData()
    {
        var avatarsDataList = SaveService.Load<AvatarsDataList>(Constants.AvatarsDataFileName);
        var customAvatarSaver = new CustomAvatarSaver(avatarsDataList);
        var playerCloudData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string>
        {
            Constants.CloudSave.Keys.PlayerId, Constants.CloudSave.Keys.PlayerName, Constants.CloudSave.Keys.PlayerAvatarId
        });
        var avatarData = avatarsDataList.AvatarsData.FirstOrDefault(avatar => avatar.Id == playerCloudData[Constants.CloudSave.Keys.PlayerAvatarId].Value.GetAsString());
        customAvatarSaver.TryLoadAvatar(avatarData);
        var playerData = new PlayerData(playerCloudData[Constants.CloudSave.Keys.PlayerId].Value.GetAsString(),
            playerCloudData[Constants.CloudSave.Keys.PlayerName].Value.GetAsString(), avatarData?.Texture)
        {
            AvatarId = playerCloudData[Constants.CloudSave.Keys.PlayerAvatarId].Value.GetAsString()
        };
        avatarsDataList.AvatarsData = customAvatarSaver.LoadAllAvatars();
        GameContext.Instance.SetPlayerData(avatarsDataList, playerData);
        SceneManager.LoadSceneAsync(Constants.Scenes.MainMenu);
    }

    private List<Resolution> GetResolutions()
    {
        List<Resolution> resolutions = new List<Resolution>
        {
            Screen.resolutions[0]
        };
        for (int i = 1; i < Screen.resolutions.Length; i++)
        {
            if (IsResolutionsSame(resolutions[^1], Screen.resolutions[i]) == false)
            {
                resolutions.Add(Screen.resolutions[i]);
            }
        }
        return resolutions;
    }

    private byte GetSystemLanguageIndex()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.English:
                return 0;
            case SystemLanguage.Russian:
                return 1;
            case SystemLanguage.Ukrainian:
                return 2;
        }
        return 0;
    }

    private bool IsResolutionsSame(Resolution firstResolution, Resolution secondResolution)
    {
        return firstResolution.width == secondResolution.width
            && firstResolution.height == secondResolution.height;
    }
}