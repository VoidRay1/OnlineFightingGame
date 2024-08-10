using System;
using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsTab : TabView
{
    [SerializeField] private Slider _volumeSlider;

    private AudioClip _buttonClickedAudioClip;

    public event Action<float> OnVolumeChanged;

    public void Initialize(SettingsData settingsData, AudioClip buttonClickedAudioClip)
    {
        _buttonClickedAudioClip = buttonClickedAudioClip;
        ResetSettings(settingsData);
    }

    public void ResetSettings(SettingsData settingsData)
    {
        _volumeSlider.SetValueWithoutNotify(settingsData.Volume);
    }

    public override void Show()
    {
        _volumeSlider.onValueChanged.AddListener(VolumeChanged);
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        _volumeSlider.onValueChanged.RemoveListener(VolumeChanged);
    }

    private void VolumeChanged(float volume)
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        OnVolumeChanged?.Invoke(volume);
    }
}