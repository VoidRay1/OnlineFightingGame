using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightRoundsToggler : MonoBehaviour
{
    [SerializeField] private FightRoundTogglesCreator _fightRoundsCreator;

    private List<Toggle> _fightRoundToggles;

    public void SetFightRoundToggles(List<Toggle> fightRoundToggles)
    {
        _fightRoundToggles = fightRoundToggles;
    }

    public void TurnOnTheRoundWinSwitch()
    {
        foreach (Toggle toggle in _fightRoundToggles)
        {
            if(toggle.isOn == false)
            {
                toggle.isOn = true;
                break;
            }
        }
    }
}