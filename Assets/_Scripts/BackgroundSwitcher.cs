using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackgroundSwitcher : MonoBehaviour
{
    [SerializeField] private List<Sprite> _backgroundSprites;
    [SerializeField] private Image _currentBackgroundImage;
    [SerializeField] private Image _nextBackgroundImage;
    [SerializeField] private float _timeToSwitchBackground;
    [SerializeField] private float _timeToDisapearCurrentBackground;

    private short _backgroundSpriteIndex = 0;

    private void Start()
    {
        SetNextBackground();
    }

    private void SetNextBackground()
    {
        _currentBackgroundImage.sprite = _backgroundSprites[_backgroundSpriteIndex];
        _backgroundSpriteIndex++;
        if (_backgroundSpriteIndex == _backgroundSprites.Count)
        {
            _backgroundSpriteIndex = 0;
        }
        _nextBackgroundImage.sprite = _backgroundSprites[_backgroundSpriteIndex];
        StartCoroutine(StartTimerToSwitchBackground());
    }

    private IEnumerator StartTimerToSwitchBackground()
    {
        float passedTime = 0.0f;
        while (passedTime < _timeToSwitchBackground)
        {
            passedTime += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(DisapearCurrentBackground());
    }

    private IEnumerator DisapearCurrentBackground()
    {
        float passedTime = 0.0f;
        while (passedTime < _timeToDisapearCurrentBackground)
        {
            passedTime += Time.deltaTime;
            _currentBackgroundImage.color = new Color(_currentBackgroundImage.color.r, _currentBackgroundImage.color.g, _currentBackgroundImage.color.b,
                1.0f - passedTime / _timeToDisapearCurrentBackground);
            yield return null;
        }
        _currentBackgroundImage.color = new Color(_currentBackgroundImage.color.r, _currentBackgroundImage.color.g, _currentBackgroundImage.color.b, 1.0f);
        SetNextBackground();
    }
}