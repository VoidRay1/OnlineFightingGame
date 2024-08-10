using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeforeFightActionsDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _beforeFightPreparationText;
    [SerializeField, Range(0.1f, 1.0f)] private float _delayBeforeTextDisapear;
    [SerializeField, Range(0.1f, 1.0f)] private float _timeToApearText;
    [SerializeField, Range(0.1f, 1.0f)] private float _timeToDisapearText; 

    public event Action OnBeforeFightActionsEnded;

    public void DisplayBeforeFightPreparationsTexts(Queue<string> textsToDisplay)
    {
        StartCoroutine(DisplayTexts(textsToDisplay));
    }

    private IEnumerator DisplayTexts(Queue<string> textsToDisplay)
    {
        while(textsToDisplay.Count > 0)
        {
            _beforeFightPreparationText.text = textsToDisplay.Dequeue();
            yield return StartCoroutine(ShowAndHideText());
        }
        OnBeforeFightActionsEnded?.Invoke();
    }

    private IEnumerator ShowAndHideText()
    {
        float passedTime = 0.0f;
        while (passedTime < _timeToApearText)
        {
            passedTime += Time.deltaTime;
            _beforeFightPreparationText.alpha += Time.deltaTime / _timeToApearText;
            yield return null;
        }
        _beforeFightPreparationText.alpha = 1.0f;

        yield return new WaitForSeconds(_delayBeforeTextDisapear);

        passedTime = 0.0f;
        while (passedTime < _timeToDisapearText)
        {
            passedTime += Time.deltaTime;
            _beforeFightPreparationText.alpha -= Time.deltaTime / _timeToDisapearText;
            yield return null;
        }
        _beforeFightPreparationText.alpha = 0.0f;
    }
}