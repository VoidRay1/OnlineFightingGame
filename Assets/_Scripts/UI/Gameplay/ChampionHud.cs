using UnityEngine;

public class ChampionHud : MonoBehaviour
{
    [SerializeField] private ChampionHealthDisplayer _championHealthDisplayer;
    [SerializeField] private ChampionStaminaDisplayer _championStaminaDisplayer;
    [SerializeField] private ChampionStateDisplayer _championStateDisplayer;
    [SerializeField] private FightRoundsToggler _fightRoundsDisplayer;

    public ChampionHealthDisplayer ChampionHealthDisplayer => _championHealthDisplayer;
    public ChampionStaminaDisplayer ChampionStaminaDisplayer => _championStaminaDisplayer;
    public ChampionStateDisplayer ChampionStateDisplayer => _championStateDisplayer;
    public FightRoundsToggler FightRoundsToggler => _fightRoundsDisplayer;
}