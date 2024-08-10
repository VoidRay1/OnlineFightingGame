using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SettingsMenuTabs : MultiTabsView
{
    [SerializeField] private Button _qualitySettingsButton;
    [SerializeField] private Button _audioSettingsButton;
    [SerializeField] private Button _languageSettingsButton;
    [SerializeField] private Button _applySettingsButton;
    [SerializeField] private Button _discardSettingsChangesButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private QualitySettingsTab _qualitySettingsTab;
    [SerializeField] private AudioSettingsTab _audioSettingsTab;
    [SerializeField] private LanguageSettingsTab _languageSettingsTab;

    private TabView _activeTab;
    private AudioClip _buttonClickedAudioClip;

    public event Action<byte> OnResolutionIndexChanged;
    public event Action<bool> OnFullScreenModeChanged;
    public event Action<bool> OnVsyncChanged;
    public event Action<short> OnFramerateChanged;
    public event Action<byte> OnQualityPresetIndexChanged;
    public event Action<float> OnVolumeChanged;
    public event Action<byte> OnLanguageIndexChanged;
    public event Action OnApplySettings;
    public event Action OnDiscardSettingsChanges;
    public event Action OnSettingsMenuClose;
    public event Action OnSettingsMenuTabsDestroyed;

    private void Awake()
    {
        Canvas.enabled = false;
    }

    public void Initialize(List<string> resolutions, List<string> qualityPresets, List<string> languages, SettingsData settingsData)
    {
        SetTabs(new List<TabView> { _qualitySettingsTab, _audioSettingsTab, _languageSettingsTab });
        HideTabs();
        GameContext.Instance.SettingsApplier.SetSettingsMenu(this);
        _buttonClickedAudioClip = GameContext.Instance.AudioClipFactory.GetAudioClip(AudioClipType.ButtonClicked);
        _qualitySettingsTab.Initialize(resolutions, qualityPresets, settingsData, _buttonClickedAudioClip);
        _audioSettingsTab.Initialize(settingsData, _buttonClickedAudioClip);
        _languageSettingsTab.Initialize(languages, settingsData, _buttonClickedAudioClip);
        SubscribeOnEvents();
        DisableApplySettingsButton();
        DisableDiscardSettingsChangesButton();
    }

    public void ResetSettings(SettingsData settingsData)
    {
        _qualitySettingsTab.ResetSettings(settingsData);
        _audioSettingsTab.ResetSettings(settingsData);
        _languageSettingsTab.ResetSettings(settingsData);
        DisableApplySettingsButton();
        DisableDiscardSettingsChangesButton();
    }

    public void EnableDiscardSettingsChangesButton()
    {
        EnableButton(_discardSettingsChangesButton);
    }

    public void DisableDiscardSettingsChangesButton()
    {
        DisableButton(_discardSettingsChangesButton);
    }

    public void EnableApplySettingsButton()
    {
        EnableButton(_applySettingsButton);
    }

    public void DisableApplySettingsButton()
    {
        DisableButton(_applySettingsButton);
    }

    public override void Hide()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        base.Hide();
        UnSubscribeOnEvents();
    }

    public void ShowQualitySettingsTab()
    {
        _activeTab = _qualitySettingsTab;
        ShowTabWithNavigationOnDown(_qualitySettingsTab);
        base.Show();
    }

    public void ShowAudioSettingsTab()
    {
        _activeTab = _audioSettingsTab;
        ShowTabWithNavigationOnDown(_audioSettingsTab);
        base.Show();
    }

    public void ShowLanguageSettingsTab()
    {
        _activeTab = _languageSettingsTab;
        ShowTabWithNavigationOnDown(_languageSettingsTab);
        base.Show();
    }

    private void EnableButton(Button button)
    {
        if (button.gameObject.activeSelf)
        {
            return;
        }
        if (_activeTab != null)
        {
            _activeTab.LastSelectable.SetSelectableOnDown(button);
            Navigation navigation = button.navigation;
            navigation.selectOnUp = _activeTab.LastSelectable;
            button.navigation = navigation;
        }
        button.gameObject.SetActive(true);
    }

    private void DisableButton(Button button)
    {
        if (button.gameObject.activeSelf == false)
        {
            return;
        }
        if (_activeTab != null)
        {
            _activeTab.LastSelectable.SetSelectableOnDown(null);
            if (EventSystem.current.currentSelectedGameObject.name == button.name)
            {
                EventSystem.current.SetSelectedGameObject(_activeTab.ActiveButton.gameObject);
            }
        }
        button.gameObject.SetActive(false);
    }

    private void ShowQualitySettingsTabWithSound()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        ShowQualitySettingsTab();
    }

    private void ShowAudioSettingsTabWithSound()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        ShowAudioSettingsTab();
    }

    private void ShowLanguageSettingsTabWithSound()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        ShowLanguageSettingsTab();
    }

    private void ApplySettings()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        OnApplySettings?.Invoke();
    }

    private void DiscardSettingsChanges()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        OnDiscardSettingsChanges?.Invoke();
    }

    private void CloseSettingsMenuTabs()
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        OnSettingsMenuClose?.Invoke();
    }

    private void VolumeChanged(float volume)
    {
        OnVolumeChanged?.Invoke(volume);
    }

    private void QualityIndexChanged(byte qualityPresetIndex)
    {
        OnQualityPresetIndexChanged?.Invoke(qualityPresetIndex);
    }

    private void FramerateChanged(short framerate)
    {
        OnFramerateChanged?.Invoke(framerate);
    }

    private void FullScreenModeChanged(bool isFullScreen)
    {
        OnFullScreenModeChanged?.Invoke(isFullScreen);
    }

    private void VsyncChanged(bool isVsyncEnabled)
    {
        OnVsyncChanged?.Invoke(isVsyncEnabled);
    }

    private void ResolutionIndexChanged(byte resolutionIndex)
    {
        OnResolutionIndexChanged?.Invoke(resolutionIndex);
    }

    private void LanguageIndexChanged(byte languageIndex)
    {
        OnLanguageIndexChanged?.Invoke(languageIndex);
    }

    private void SubscribeOnEvents()
    {
        _qualitySettingsButton.onClick.AddListener(ShowQualitySettingsTabWithSound);
        _qualitySettingsTab.OnResolutionIndexChanged += ResolutionIndexChanged;
        _qualitySettingsTab.OnFullScreenModeChanged += FullScreenModeChanged;
        _qualitySettingsTab.OnVsyncChanged += VsyncChanged;
        _qualitySettingsTab.OnFramerateChanged += FramerateChanged;
        _qualitySettingsTab.OnQualityPresetIndexChanged += QualityIndexChanged;
        _audioSettingsButton.onClick.AddListener(ShowAudioSettingsTabWithSound);
        _audioSettingsTab.OnVolumeChanged += VolumeChanged;
        _languageSettingsButton.onClick.AddListener(ShowLanguageSettingsTabWithSound);
        _languageSettingsTab.OnLanguageIndexChanged += LanguageIndexChanged;
        _discardSettingsChangesButton.onClick.AddListener(DiscardSettingsChanges);
        _applySettingsButton.onClick.AddListener(ApplySettings);
        _closeButton.onClick.AddListener(CloseSettingsMenuTabs);
    }

    private void UnSubscribeOnEvents()
    {
        _qualitySettingsButton.onClick.RemoveListener(ShowQualitySettingsTabWithSound);
        _qualitySettingsTab.OnResolutionIndexChanged -= ResolutionIndexChanged;
        _qualitySettingsTab.OnFullScreenModeChanged -= FullScreenModeChanged;
        _qualitySettingsTab.OnVsyncChanged -= VsyncChanged;
        _qualitySettingsTab.OnFramerateChanged -= FramerateChanged;
        _qualitySettingsTab.OnQualityPresetIndexChanged -= QualityIndexChanged;
        _audioSettingsButton.onClick.RemoveListener(ShowAudioSettingsTabWithSound);
        _audioSettingsTab.OnVolumeChanged -= VolumeChanged;
        _languageSettingsButton.onClick.RemoveListener(ShowLanguageSettingsTabWithSound);
        _languageSettingsTab.OnLanguageIndexChanged -= LanguageIndexChanged;
        _discardSettingsChangesButton.onClick.RemoveListener(DiscardSettingsChanges);
        _applySettingsButton.onClick.RemoveListener(ApplySettings);
        _closeButton.onClick.RemoveListener(CloseSettingsMenuTabs);
    }

    private void OnDestroy()
    { 
        UnSubscribeOnEvents();
        OnSettingsMenuTabsDestroyed?.Invoke();
    }
}