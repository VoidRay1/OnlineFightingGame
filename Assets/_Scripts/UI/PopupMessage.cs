using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class PopupMessage : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField, Range(0.1f, 10.0f)] private float _timeToFadeInMessage;
    [SerializeField, Range(0.1f, 10.0f)] private float _fadeInDelay; 
    [SerializeField] private Color _successfulColor = Color.green;
    [SerializeField] private Color _neutralColor = Color.white;
    [SerializeField] private Color _errorColor = Color.red;

    public event Action OnFadeInMessageEnded;

    public void ShowSuccessfulMessage(string text, bool showInstant = true)
    {
        _text.color = new Color(_successfulColor.r, _successfulColor.g, _successfulColor.b, 0.0f);
        _text.text = text;
        StartFade(showInstant);
    }

    public void ShowNeutralMessage(string text, bool showInstant = true)
    {
        _text.color = new Color(_neutralColor.r, _neutralColor.g, _neutralColor.b, 0.0f);
        _text.text = text;
        StartFade(showInstant);
    }

    public void ShowErrorMessage(string text, bool showInstant = true)
    {
        _text.color = new Color(_errorColor.r, _errorColor.g, _errorColor.b, 0.0f);
        _text.text = text;
        StartFade(showInstant);
    }

    private void StartFade(bool showInstant)
    {
        if (showInstant)
        {
            _text.alpha = 1.0f;
            StartCoroutine(WaitForFadeInDelay());
        }
    }

    private IEnumerator WaitForFadeInDelay()
    {
        yield return new WaitForSeconds(_fadeInDelay);
        StartCoroutine(FadeInMessage());
    }

    private IEnumerator FadeInMessage()
    {
        float passedTime = 0.0f;
        while (passedTime < _timeToFadeInMessage)
        {
            passedTime += Time.deltaTime;
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1.0f - passedTime / _timeToFadeInMessage);
            yield return null;
        }
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 0.0f);
        OnFadeInMessageEnded?.Invoke();
    }
}