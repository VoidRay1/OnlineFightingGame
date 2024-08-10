using System.Collections;
using UnityEngine;

public class FightTimer : MonoBehaviour
{
    [SerializeField, Range(30, 120)] private byte _timeForFight = 90;
    [SerializeField] private FightTimerDisplayer _fightTimerDisplayer;

    private byte _currentFightTime;
    private Coroutine _startTimerCoroutine;
    private const byte DecreaseTime = 1;

    public void ResetTimer()
    {
        _currentFightTime = _timeForFight;
        _fightTimerDisplayer.Display(_currentFightTime);
    }

    public void Run()
    {
        ResetTimer();
        _startTimerCoroutine = StartCoroutine(StartTimer());
    }

    public void Stop()
    {
        if (_startTimerCoroutine != null) 
        {
            StopCoroutine(_startTimerCoroutine);
        }
    }

    private IEnumerator StartTimer()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(DecreaseTime);
        while(_currentFightTime != 0)
        {
            _currentFightTime -= DecreaseTime;
            _fightTimerDisplayer.Display(_currentFightTime);
            yield return waitForSeconds;
        }
    }
}