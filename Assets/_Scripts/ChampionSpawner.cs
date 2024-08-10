using Unity.Netcode;
using UnityEngine;

public class ChampionSpawner : MonoBehaviour
{
    [SerializeField] private ChampionFactory _championFactory;
    [SerializeField] private ChampionSpawnPoint _firstSpawnPoint;
    [SerializeField] private ChampionSpawnPoint _secondSpawnPoint;

    public (Champion firstChampion, Champion secondChampion) Spawn(ChampionType firstChampionType, ChampionType secondChampionType)
    {
        Champion firstChampion = _championFactory.GetTrainingChampion(firstChampionType);
        Champion secondChampion = _championFactory.GetTrainingChampion(secondChampionType);
        Champion createdFirstChampion = Instantiate(firstChampion, _firstSpawnPoint.Position, _firstSpawnPoint.GetViewRotation());
        Champion createdSecondChampion = Instantiate(secondChampion, _secondSpawnPoint.Position, _secondSpawnPoint.GetViewRotation());
        createdFirstChampion.SetViewDirection(_firstSpawnPoint.ViewDirection);
        createdSecondChampion.SetViewDirection(secondChampion.ViewDirection);
        return (createdFirstChampion, createdSecondChampion);
    }

    public Champion SpawnHostChampion(ChampionType championType)
    {
        Champion champion = _championFactory.GetOnlineChampion(championType);
        Champion createdChampion = Instantiate(champion, _firstSpawnPoint.Position, _firstSpawnPoint.GetViewRotation());
        createdChampion.NetworkObject.SpawnAsPlayerObject(NetworkManager.ServerClientId, true);
        return createdChampion;
    }

    public Champion SpawnClientChampion(ChampionType championType, ulong clientId, bool spawnOnFirstPoint)
    {
        Champion champion = _championFactory.GetOnlineChampion(championType);
        Champion createdChampion = Instantiate(champion, 
            spawnOnFirstPoint ? _firstSpawnPoint.Position : _secondSpawnPoint.Position,
            spawnOnFirstPoint ? _firstSpawnPoint.GetViewRotation() : _secondSpawnPoint.GetViewRotation());
        createdChampion.SetViewDirection(spawnOnFirstPoint ? ViewDirection.Right : ViewDirection.Left);
        createdChampion.NetworkObject.SpawnAsPlayerObject(clientId, true);
        return createdChampion;
    }
}