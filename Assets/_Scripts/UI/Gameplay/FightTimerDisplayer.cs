using TMPro;
using UnityEngine;

public class FightTimerDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _timerText;

    public void Display(int time)
    {
        _timerText.text = time.ToString();
    }
}