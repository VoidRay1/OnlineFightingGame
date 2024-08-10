using System;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSettingsTab : TabView
{
    [SerializeField] private DropdownWithArrows _languagesDropdown;

    private AudioClip _buttonClickedAudioClip;

    public event Action<byte> OnLanguageIndexChanged;

    public void Initialize(List<string> languages, SettingsData settingsData, AudioClip buttonClickedAudioClip)
    {
        _languagesDropdown.Initialize(languages);
        _buttonClickedAudioClip = buttonClickedAudioClip;
        ResetSettings(settingsData);
    }

    public override void Show()
    {
        _languagesDropdown.Dropdown.onValueChanged.AddListener(LanguageIndexChanged);
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        _languagesDropdown.Dropdown.onValueChanged.RemoveListener(LanguageIndexChanged);
    }

    private void LanguageIndexChanged(int languageIndex)
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        OnLanguageIndexChanged?.Invoke((byte)languageIndex);
    }

    public void ResetSettings(SettingsData settingsData)
    {
        _languagesDropdown.Dropdown.value = settingsData.LanguageIndex;
    }
}