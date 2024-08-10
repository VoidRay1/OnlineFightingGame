using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QualitySettingsTab : TabView
{
    [SerializeField] private DropdownWithArrows _resolutionsDropdown;
    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private Toggle _vsyncToggle;
    [SerializeField] private TMP_InputField _framerateInput;
    [SerializeField] private Slider _framerateSlider;
    [SerializeField] private DropdownWithArrows _qualityPresetsDropdown;

    private AudioClip _buttonClickedAudioClip;
    private const byte FramerateSliderDecreaseValue = 10;

    public event Action<byte> OnResolutionIndexChanged;
    public event Action<bool> OnFullScreenModeChanged;
    public event Action<bool> OnVsyncChanged;
    public event Action<short> OnFramerateChanged;
    public event Action<byte> OnQualityPresetIndexChanged;

    public void Initialize(List<string> resolutions, List<string> qualityPresets, SettingsData settingsData, AudioClip buttonClickedAudioClip)
    {
        _resolutionsDropdown.Initialize(resolutions);
        _qualityPresetsDropdown.Initialize(qualityPresets);
        _framerateSlider.minValue = Constants.MinFramerate / FramerateSliderDecreaseValue;
        _framerateSlider.maxValue = Constants.MaxFramerate / FramerateSliderDecreaseValue;
        _buttonClickedAudioClip = buttonClickedAudioClip;
        ResetSettings(settingsData);
    }

    public void ResetSettings(SettingsData settingsData)
    {
        _resolutionsDropdown.Dropdown.value = settingsData.ResolutionIndex;
        _framerateInput.text = settingsData.Framerate.ToString();
        _framerateSlider.SetValueWithoutNotify(settingsData.Framerate / FramerateSliderDecreaseValue);
        _qualityPresetsDropdown.Dropdown.value = settingsData.QualityPresetIndex;
        _fullScreenToggle.isOn = settingsData.IsFullScreenMode;
        _vsyncToggle.isOn = settingsData.IsVsyncEnabled;
    }

    public override void Show()
    {
        SubscribeOnEvents();
        base.Show();
    }

    public override void Hide()
    {
        base.Hide();
        UnSubscribeOnEvents();
    }

    private void DropdownOpened()
    {
        HasOpenedSelectable = true;
    }

    private void DropdownClosed()
    {
        HasOpenedSelectable = false;
    }

    private void InputFramerateChanged(string framerate)
    {
        if (framerate.Length >= 1)
        {
            string parsedFramerate = Regex.Replace(framerate, "-", "");
            int zerosCount = 0;
            for (int i = 0; i < parsedFramerate.Length; i++)
            {
                if (parsedFramerate[i] == '0')
                {
                    zerosCount++;
                }
                else
                {
                    break;
                }
            }
            if (parsedFramerate.Length < framerate.Length || zerosCount > 0)
            {
                _framerateInput.stringPosition--;
            }
            _framerateInput.SetTextWithoutNotify(parsedFramerate[zerosCount..]);
        }
    }

    private void InputFramerateEndEdit(string framerate)
    {
        if (short.TryParse(framerate, out short parsedFramerate))
        {
            if (parsedFramerate < Constants.MinFramerate)
            {
                _framerateInput.SetTextWithoutNotify(Constants.MinFramerate.ToString());
                _framerateSlider.value = Constants.MinFramerate;
            }
            else if (parsedFramerate > Constants.MaxFramerate)
            {
                _framerateInput.SetTextWithoutNotify(Constants.MaxFramerate.ToString());
                _framerateSlider.value = Constants.MaxFramerate;
            }
            _framerateSlider.value = parsedFramerate / FramerateSliderDecreaseValue;
            OnFramerateChanged?.Invoke(parsedFramerate);
        }
    }

    private void SliderFramerateChanged(float framerate)
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        short result = (short)(framerate * FramerateSliderDecreaseValue);
        OnFramerateChanged?.Invoke(result);
        _framerateInput.SetTextWithoutNotify(result.ToString());
    }

    private void ResolutionIndexChanged(int resolutionIndex)
    {
        OnResolutionIndexChanged?.Invoke((byte)resolutionIndex);
    }

    private void QualityPresetIndexChanged(int qualityPresetIndex)
    {
        OnQualityPresetIndexChanged?.Invoke((byte)qualityPresetIndex);
    }

    private void FullScreenModeChanged(bool isFullScreen)
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        OnFullScreenModeChanged?.Invoke(isFullScreen);
    }

    private void VsyncChanged(bool isVsyncEnabled)
    {
        GameContext.Instance.AudioSourcePlayer.PlayClip(_buttonClickedAudioClip);
        OnVsyncChanged?.Invoke(isVsyncEnabled);
    }

    private void SubscribeOnEvents()
    {
        _resolutionsDropdown.OnDropdownOpened += DropdownOpened;
        _resolutionsDropdown.OnDropdownClosed += DropdownClosed;
        _resolutionsDropdown.Dropdown.onValueChanged.AddListener(ResolutionIndexChanged);
        _framerateInput.onValueChanged.AddListener(InputFramerateChanged);
        _framerateInput.onEndEdit.AddListener(InputFramerateEndEdit);
        _framerateSlider.onValueChanged.AddListener(SliderFramerateChanged);
        _fullScreenToggle.onValueChanged.AddListener(FullScreenModeChanged);
        _vsyncToggle.onValueChanged.AddListener(VsyncChanged);
        _qualityPresetsDropdown.Dropdown.onValueChanged.AddListener(QualityPresetIndexChanged);
        _qualityPresetsDropdown.OnDropdownOpened += DropdownOpened;
        _qualityPresetsDropdown.OnDropdownClosed += DropdownClosed;
    }

    private void UnSubscribeOnEvents()
    {
        _resolutionsDropdown.OnDropdownOpened -= DropdownOpened;
        _resolutionsDropdown.OnDropdownClosed -= DropdownClosed;
        _resolutionsDropdown.Dropdown.onValueChanged.RemoveListener(ResolutionIndexChanged);
        _framerateInput.onValueChanged.RemoveListener(InputFramerateChanged);
        _framerateInput.onEndEdit.RemoveListener(InputFramerateEndEdit);
        _framerateSlider.onValueChanged.RemoveListener(SliderFramerateChanged);
        _fullScreenToggle.onValueChanged.RemoveListener(FullScreenModeChanged);
        _vsyncToggle.onValueChanged.RemoveListener(VsyncChanged);
        _qualityPresetsDropdown.Dropdown.onValueChanged.RemoveListener(QualityPresetIndexChanged);
        _qualityPresetsDropdown.OnDropdownOpened -= DropdownOpened;
        _qualityPresetsDropdown.OnDropdownClosed -= DropdownClosed;
    }

    private void OnDestroy()
    {
        UnSubscribeOnEvents();
    }
}