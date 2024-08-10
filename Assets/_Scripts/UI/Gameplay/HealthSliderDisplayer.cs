using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthSliderDisplayer : MonoBehaviour
{
    [SerializeField] private Slider _healthSlider;
    [SerializeField] private Image _healthImage;
    [SerializeField] private Color _blinkColor;
    [SerializeField, Range(0.1f, 1.0f)] private float _timeToBlink;
    private Color _originalColor;
    private float _minimumAmountOfHealthForSliderBlinking;
    private IEnumerator _activeBlinkingCoroutine;
    private float _healthSliderBlinkingSpeedIncrease;
    private float _previousHealthAmount;
    private float _originalTimeToBlink;
    private const byte TimesToReduceHealthForSliderBlinking = 4;
    private const byte PercentsAmountToDecreaseTimeForHealthSliderBlinking = 5;
    private const byte TimesToIncreaseHealthSliderBlinkingSpeed = 8;

    public void Initialize()
    {
        _originalTimeToBlink = _timeToBlink;
        _originalColor = _healthImage.color;
        _healthSliderBlinkingSpeedIncrease = _timeToBlink / TimesToIncreaseHealthSliderBlinkingSpeed;
    }

    public void SetHealthSliderMaxValue(float maxValue)
    {
        _healthSlider.maxValue = maxValue;
        _minimumAmountOfHealthForSliderBlinking = maxValue / TimesToReduceHealthForSliderBlinking;
    }

    public void Reset()
    {
        _healthImage.color = _originalColor;
        _healthSlider.value = _healthSlider.maxValue;
        _previousHealthAmount = _healthSlider.value;
        _timeToBlink = _originalTimeToBlink;
        if(_activeBlinkingCoroutine is not null) 
        {
            StopCoroutine(_activeBlinkingCoroutine);
            _activeBlinkingCoroutine = null;
        }
    }

    public void SetHealthSliderValue(float value)
    {
        if (IsItPossibleToStartBlinking(value))
        {
            _activeBlinkingCoroutine = StartBlinking();
            StartCoroutine(_activeBlinkingCoroutine);
        }
        if (TryIncreaseBlinkingSpeed(value))
        {
            _timeToBlink -= _healthSliderBlinkingSpeedIncrease * Mathf.FloorToInt((_previousHealthAmount - value) / PercentsAmountToDecreaseTimeForHealthSliderBlinking);
        }
        _previousHealthAmount = value;
        _healthSlider.value = value;
    }

    private IEnumerator StartBlinking()
    {
        float passedTime;
        while (true)
        {
            passedTime = 0.0f;
            while (passedTime < _timeToBlink)
            {
                passedTime += Time.deltaTime;
                _healthImage.color = Color.Lerp(_originalColor, _blinkColor, passedTime / _timeToBlink);
                yield return null;
            }

            passedTime = 0.0f;
            while (passedTime < _timeToBlink)
            {
                passedTime += Time.deltaTime;
                _healthImage.color = Color.Lerp(_blinkColor, _originalColor, passedTime / _timeToBlink);
                yield return null;
            }
        }
    }

    private bool IsItPossibleToStartBlinking(float value)
    {
        return _activeBlinkingCoroutine is null && IsHealhBelowMinimumForSliderBlinking(value);
    }

    private bool IsHealhBelowMinimumForSliderBlinking(float value)
    {
        return value < _minimumAmountOfHealthForSliderBlinking;
    }

    private bool TryIncreaseBlinkingSpeed(float value)
    {
        return IsHealhBelowMinimumForSliderBlinking(value)
            && Mathf.CeilToInt((_minimumAmountOfHealthForSliderBlinking - value) / PercentsAmountToDecreaseTimeForHealthSliderBlinking) > 1;
    }
}        