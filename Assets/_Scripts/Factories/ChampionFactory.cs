using UnityEngine;

[CreateAssetMenu(menuName = "Factories/Champion Factory")]
public class ChampionFactory : ScriptableObject
{
    [SerializeField] private Champion _trainingSwatChampion;
    [SerializeField] private Champion _onlineSwatChampion;

    public Champion GetTrainingChampion(ChampionType championType)
    {
        switch (championType)
        {
            case ChampionType.Swat:
                return _trainingSwatChampion;
        }
        return null;
    }

    public Champion GetOnlineChampion(ChampionType championType)
    {
        switch (championType)
        {
            case ChampionType.Swat:
                return _onlineSwatChampion;
        }
        return null;
    }
}