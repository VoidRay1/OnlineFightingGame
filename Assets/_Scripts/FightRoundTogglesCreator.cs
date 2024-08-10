using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FightRoundTogglesCreator : MonoBehaviour
{
    [SerializeField] private Toggle _fightRoundTogglePrefab;
    [SerializeField] private FightRoundsToggler _firstChampionFightRoundsToggler;
    [SerializeField] private FightRoundsToggler _secondChampionFightRoundsToggler;
    [SerializeField] private Transform _firstChampionFightRoundsToggleParent;
    [SerializeField] private Transform _secondChampionFightRoundsToggleParent;

    public void CreateFightRoundToggles(int fightRounds)
    {
        List<Toggle> firstChampionFightRoundToggles = new List<Toggle>(fightRounds);
        List<Toggle> secondChampionFightRoundToggles = new List<Toggle>(fightRounds);
        for(int i = 0; i < fightRounds; i++)
        {
            Toggle firstChampionFightRoundToggle = Instantiate(_fightRoundTogglePrefab, _firstChampionFightRoundsToggleParent);
            Toggle secondChampionFightRoundToggle = Instantiate(_fightRoundTogglePrefab, _secondChampionFightRoundsToggleParent);
            firstChampionFightRoundToggles.Add(firstChampionFightRoundToggle);
            secondChampionFightRoundToggles.Add(secondChampionFightRoundToggle);
        }
        _firstChampionFightRoundsToggler.SetFightRoundToggles(firstChampionFightRoundToggles);
        _secondChampionFightRoundsToggler.SetFightRoundToggles(secondChampionFightRoundToggles);
    }
}