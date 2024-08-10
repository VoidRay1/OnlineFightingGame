using System.Collections;
using UnityEngine;

public class FadeInOutAnimation : MonoBehaviour
{
    [SerializeField, Range(0.1f, 10.0f)] private float _maxTimeToFadeIn;
    [SerializeField, Range(0.1f, 10.0f)] private float _maxTimeToFadeOut;

    private IEnumerator _activeCoroutine;

/*    public void StartFadeIn()
    {
        _activeCoroutine = StartFadeInCoroutine();
    }

    public void StartFadeOut()
    {
        _activeCoroutine = StartFadeOutCoroutine();
    }

    public void StartFadeInFadeOut()
    {
        _activeCoroutine = StartFadeInOutCoroutine();
    }

    private IEnumerator StartFadeInOutCoroutine()
    {

    }

    private IEnumerator StartFadeInCoroutine()
    {

    }

    private IEnumerator StartFadeOutCoroutine()
    {

    }*/
}