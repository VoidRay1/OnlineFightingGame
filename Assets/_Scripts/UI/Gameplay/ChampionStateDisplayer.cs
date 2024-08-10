using TMPro;
using UnityEngine;

public class ChampionStateDisplayer : MonoBehaviour
{
    [SerializeField] private TMP_Text _championStateText;

    public void DisplayCurrentState(string state)
    {
        _championStateText.text = state;
    }

    public void DisplayTransitionBetweenStates(string from, string to)
    {
        _championStateText.text = $"{from}->{to}";
    }
}