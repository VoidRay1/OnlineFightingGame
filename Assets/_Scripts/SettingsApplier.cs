using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class SettingsApplier
{
    private readonly ISaveService _saveService;
    private SettingsMenuTabs _settingsMenu;
    private ConfirmationWindow _confirmationWindow;
    private SettingsData _originalSaveData;
    private bool _isSettinsApplied;

    private ChangedSettingValue<byte> _resolutionIndexSetting = new ChangedSettingValue<byte>();
    private ChangedSettingValue<bool> _fullScreenModeSetting = new ChangedSettingValue<bool>();
    private ChangedSettingValue<bool> _vsyncSetting = new ChangedSettingValue<bool>();
    private ChangedSettingValue<short> _framerateSetting = new ChangedSettingValue<short>();
    private ChangedSettingValue<byte> _qualityPresetIndexSetting = new ChangedSettingValue<byte>();
    private ChangedSettingValue<float> _volumeSetting = new ChangedSettingValue<float>();
    private ChangedSettingValue<byte> _languageIndexSetting = new ChangedSettingValue<byte>();

    private ConfirmationWindowProvider ConfirmationWindowProvider => GameContext.Instance.ConfirmationWindowProvider;
    private WindowCloser WindowCloser => GameContext.Instance.WindowCloser;
    private SettingsData SettingsData => GameContext.Instance.SettingsData;

    private const string SettingsMenuTable = Constants.Localization.TableReferences.SettingsMenuTable;
    private const string ConfirmText = "Save";
    private const string CancelText = "Discard";
    private const string Description = "Save settings changes?";

    public SettingsApplier(ISaveService saveService)
    {
        _saveService = saveService;
    }

    public void SetSettingsMenu(SettingsMenuTabs settingsMenu)
    {
        _settingsMenu = settingsMenu;
        _originalSaveData = SettingsData.ShallowCopy();
        _isSettinsApplied = false;
        _resolutionIndexSetting.OriginalValue = SettingsData.ResolutionIndex;
        _fullScreenModeSetting.OriginalValue = SettingsData.IsFullScreenMode;
        _vsyncSetting.OriginalValue = SettingsData.IsVsyncEnabled;
        _framerateSetting.OriginalValue = SettingsData.Framerate;
        _qualityPresetIndexSetting.OriginalValue = SettingsData.QualityPresetIndex;
        _volumeSetting.OriginalValue = SettingsData.Volume;
        _languageIndexSetting.OriginalValue = SettingsData.LanguageIndex;
        WindowCloser.Disable();
        SubscribeOnEvents();
    }

    private async void TryCloseSettingsMenuWithButton()
    {
        if (await TryShowConfirmationWindow() == false)
        {
            WindowCloser.TryHideWindow();
            WindowCloser.Enable();
        }
    }

    private async void TryCloseSettingsMenuWithTrigger()
    {
        if (await TryShowConfirmationWindow())
        {
            WindowCloser.Enable();
        }
        else if (_confirmationWindow == null)
        {
            WindowCloser.TryHideWindow();
            WindowCloser.Enable();
        }
    }

    private async Task<bool> TryShowConfirmationWindow()
    {
        if (CanShowConfirmationWindow())
        {
            _confirmationWindow = await ConfirmationWindowProvider.ShowConfirmationWindow(BeforeApplySettingsForConfirmationWindow, BeforeDiscardSettingsChanges,
                new LocalizedString
                {
                    TableReference = SettingsMenuTable,
                    TableEntryReference = Description
                },
                new LocalizedString
                {
                    TableReference = SettingsMenuTable,
                    TableEntryReference = ConfirmText
                },
                new LocalizedString
                {
                    TableReference = SettingsMenuTable,
                    TableEntryReference = CancelText
                });
            return true;
        }
        return false;
    }

    private void VolumeChanged(float volume)
    {
        SettingsData.Volume = volume;
        _volumeSetting.NewValue = volume;
        TryEnableApplySettingsButton();
    }

    private void QualityPresetIndexChanged(byte qualityPresetIndex)
    {
        SettingsData.QualityPresetIndex = qualityPresetIndex;
        _qualityPresetIndexSetting.NewValue = qualityPresetIndex;
        TryEnableApplySettingsButton();
    }

    private void FramerateChanged(short framerate)
    {
        SettingsData.Framerate = framerate;
        _framerateSetting.NewValue = framerate;
        TryEnableApplySettingsButton();
    }

    private void FullScreenModeChanged(bool isFullScreen)
    {
        SettingsData.IsFullScreenMode = isFullScreen;
        _fullScreenModeSetting.NewValue = isFullScreen;
        TryEnableApplySettingsButton();  
    }

    private void VsyncChanged(bool isVsyncEnabled)
    {
        SettingsData.IsVsyncEnabled = isVsyncEnabled;
        _vsyncSetting.NewValue = isVsyncEnabled;
        TryEnableApplySettingsButton();
    }

    private void ResolutionIndexChanged(byte resolutionIndex)
    {
        SettingsData.ResolutionIndex = resolutionIndex;
        _resolutionIndexSetting.NewValue = resolutionIndex;
        TryEnableApplySettingsButton();
    }

    private void LanguageIndexChanged(byte languageIndex)
    {
        SettingsData.LanguageIndex = languageIndex;
        _languageIndexSetting.NewValue = languageIndex;
        TryEnableApplySettingsButton();
    }

    private void BeforeApplySettingsForConfirmationWindow()
    {
        _confirmationWindow = null;
        ConfirmationWindowProvider.UnloadConfirmationWindow();
        BeforeApplySettings();
    }

    private void BeforeApplySettings()
    {
        _settingsMenu.DisableApplySettingsButton();
        _settingsMenu.EnableDiscardSettingsChangesButton();
        ApplySettings();
    }

    private void BeforeDiscardSettingsChanges()
    {
        _confirmationWindow = null;
        ConfirmationWindowProvider.UnloadConfirmationWindow();
        DiscardSettingsChanges();
    }

    private void ApplySettings()
    {
        WindowCloser.Enable();
        _isSettinsApplied = true;
        if (_resolutionIndexSetting.IsChanged)
        {
            _resolutionIndexSetting = new ChangedSettingValue<byte>(SettingsData.ResolutionIndex);
            ApplyResolution();
        }
        if (_fullScreenModeSetting.IsChanged)
        {
            _fullScreenModeSetting = new ChangedSettingValue<bool>(SettingsData.IsFullScreenMode);
            ApplyFullScreenMode();
        }
        if (_vsyncSetting.IsChanged)
        {
            _vsyncSetting = new ChangedSettingValue<bool>(SettingsData.IsVsyncEnabled);
            ApplyVsync();
        }
        if (_framerateSetting.IsChanged)
        {
            _framerateSetting = new ChangedSettingValue<short>(SettingsData.Framerate);
            ApplyFramerate();
        }
        if (_qualityPresetIndexSetting.IsChanged)
        {
            _qualityPresetIndexSetting = new ChangedSettingValue<byte>(SettingsData.QualityPresetIndex);
            ApplyQualityPreset();
        }
        if (_volumeSetting.IsChanged)
        {
            _volumeSetting = new ChangedSettingValue<float>(SettingsData.Volume);
            ApplyVolume();
        }
        if (_languageIndexSetting.IsChanged)
        {
            _languageIndexSetting = new ChangedSettingValue<byte>(SettingsData.LanguageIndex);
            ApplyLanguage();
        }
        _saveService.Save(Constants.SettingsFileName, SettingsData);
    }

    public void ApplyAllSettings()
    {
#if SERVER == false
        ApplyResolution();
#endif
        ApplyFullScreenMode();
        ApplyVsync();
        ApplyFramerate();
        ApplyQualityPreset();
        ApplyVolume();
        ApplyLanguage();
    }

    private void DiscardSettingsChanges()
    {
        _settingsMenu.ResetSettings(_originalSaveData);
        ApplySettings();
    }

    public void ApplyResolution()
    {
        Resolution resolutionToApply = GameContext.Instance.Resolutions[SettingsData.ResolutionIndex];
        Screen.SetResolution(resolutionToApply.width, resolutionToApply.height, SettingsData.IsFullScreenMode);
    }

    public void ApplyFullScreenMode()
    {
        FullScreenMode fullScreenMode = SettingsData.IsFullScreenMode ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.fullScreenMode = fullScreenMode;
    }

    public void ApplyVsync()
    {
        QualitySettings.vSyncCount = SettingsData.IsVsyncEnabled ? 1 : 0;
    }

    public void ApplyFramerate()
    {
        Application.targetFrameRate = SettingsData.Framerate;
    }

    public void ApplyQualityPreset()
    {
        QualitySettings.SetQualityLevel(SettingsData.QualityPresetIndex);
    }

    public void ApplyVolume()
    {
        AudioListener.volume = SettingsData.Volume;
    }

    public void ApplyLanguage()
    {
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[SettingsData.LanguageIndex];
    }

    private void SettingsMenuTabsDestroyed()
    {
        UnSubscribeOnEvents();
    }

    private void TryEnableApplySettingsButton()
    {
        if (IsSettingsChanged())
        {
            _isSettinsApplied = false;
            WindowCloser.Disable();
            _settingsMenu.EnableApplySettingsButton();
        }
        else
        {
            _isSettinsApplied = true;
            WindowCloser.Enable();
            _settingsMenu.DisableApplySettingsButton();
        }
    }

    private bool CanShowConfirmationWindow()
    {
        return IsSettingsChanged() && _isSettinsApplied == false && _confirmationWindow == null;
    }

    private bool IsSettingsChanged()
    {
        return _resolutionIndexSetting.IsChanged
            || _fullScreenModeSetting.IsChanged
            || _vsyncSetting.IsChanged
            || _framerateSetting.IsChanged
            || _qualityPresetIndexSetting.IsChanged
            || _volumeSetting.IsChanged
            || _languageIndexSetting.IsChanged;
    }

    private void SubscribeOnEvents()
    {
        WindowCloser.OnCloseWindowTrigger += TryCloseSettingsMenuWithTrigger;
        _settingsMenu.OnSettingsMenuTabsDestroyed += SettingsMenuTabsDestroyed;
        _settingsMenu.OnResolutionIndexChanged += ResolutionIndexChanged;
        _settingsMenu.OnFullScreenModeChanged += FullScreenModeChanged;
        _settingsMenu.OnVsyncChanged += VsyncChanged;
        _settingsMenu.OnFramerateChanged += FramerateChanged;
        _settingsMenu.OnQualityPresetIndexChanged += QualityPresetIndexChanged;
        _settingsMenu.OnVolumeChanged += VolumeChanged;
        _settingsMenu.OnLanguageIndexChanged += LanguageIndexChanged;
        _settingsMenu.OnApplySettings += BeforeApplySettings;
        _settingsMenu.OnDiscardSettingsChanges += DiscardSettingsChanges;
        _settingsMenu.OnSettingsMenuClose += TryCloseSettingsMenuWithButton;
    }

    private void UnSubscribeOnEvents()
    {
        WindowCloser.OnCloseWindowTrigger -= TryCloseSettingsMenuWithTrigger;
        _settingsMenu.OnSettingsMenuTabsDestroyed -= SettingsMenuTabsDestroyed;
        _settingsMenu.OnResolutionIndexChanged -= ResolutionIndexChanged;
        _settingsMenu.OnFullScreenModeChanged -= FullScreenModeChanged;
        _settingsMenu.OnVsyncChanged -= VsyncChanged;
        _settingsMenu.OnFramerateChanged -= FramerateChanged;
        _settingsMenu.OnQualityPresetIndexChanged -= QualityPresetIndexChanged;
        _settingsMenu.OnVolumeChanged -= VolumeChanged;
        _settingsMenu.OnLanguageIndexChanged -= LanguageIndexChanged;
        _settingsMenu.OnApplySettings -= BeforeApplySettings;
        _settingsMenu.OnDiscardSettingsChanges -= DiscardSettingsChanges;
        _settingsMenu.OnSettingsMenuClose -= TryCloseSettingsMenuWithButton;
    }
}