using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AppliedDamageSliderDisplayer : MonoBehaviour
{
    [SerializeField] private Slider _appliedDamageSlider;
    [SerializeField] private Image _appliedDamageImage;
    [SerializeField, Range(0.1f, 1.0f)] private float _delayBeforeAppliedDamageSliderDisapear;
    [SerializeField, Range(0.1f, 1.0f)] private float _timeToDisapearAppliedDamageSlider;

    private float _currentHealth;
    private IEnumerator _activeCountdownCoroutine;
    private IEnumerator _activeDisapearAppliedDamageImageCoroutine;

    public void Show(float currentHealth)
    {
        TryStopActiveCountdownCoroutines();
        _currentHealth = currentHealth;
        _activeCountdownCoroutine = StartCountdownToDisapearAppliedDamageImage();
        StartCoroutine(_activeCountdownCoroutine);
    }

    public void Reset()
    {
        _appliedDamageSlider.value = _appliedDamageSlider.maxValue;
        TryStopActiveCountdownCoroutines();
    }

    public void SetAppliedDamageSliderMaxValue(float maxValue)
    {
        _appliedDamageSlider.maxValue = maxValue;
    }

    public void SetAppliedDamageSliderValue(float value)
    {
        _appliedDamageSlider.value = value;
    }

    private IEnumerator StartCountdownToDisapearAppliedDamageImage()
    {
        yield return new WaitForSeconds(_delayBeforeAppliedDamageSliderDisapear);
        _activeDisapearAppliedDamageImageCoroutine = DisapearAppliedDamageImage();
        StartCoroutine(_activeDisapearAppliedDamageImageCoroutine);
    }

    private IEnumerator DisapearAppliedDamageImage()
    {
        float passedTime = 0.0f;
        while(passedTime < _timeToDisapearAppliedDamageSlider)
        {
            passedTime += Time.deltaTime;
            _appliedDamageImage.color = new Color(_appliedDamageImage.color.r, _appliedDamageImage.color.g, _appliedDamageImage.color.b,
                1.0f - passedTime / _timeToDisapearAppliedDamageSlider);
            yield return null;
        }
        _activeDisapearAppliedDamageImageCoroutine = null;
        CorrectAppliedDamageSliderValues();
    }

    private void CorrectAppliedDamageSliderValues()
    {
        _appliedDamageSlider.value = _currentHealth;
        _appliedDamageImage.color = new Color(_appliedDamageImage.color.r, _appliedDamageImage.color.g, _appliedDamageImage.color.b, 1.0f);
    }

    private void TryStopActiveCountdownCoroutines()
    {
        if(_activeCountdownCoroutine is not null)
        {
            StopCoroutine(_activeCountdownCoroutine);
            _activeCountdownCoroutine = null;
        }
        if(_activeDisapearAppliedDamageImageCoroutine is not null)
        {
            StopCoroutine(_activeDisapearAppliedDamageImageCoroutine);
            _activeDisapearAppliedDamageImageCoroutine = null;
            CorrectAppliedDamageSliderValues();
        }
    }
}