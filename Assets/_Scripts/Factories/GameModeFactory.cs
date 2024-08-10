using UnityEngine;

[CreateAssetMenu(menuName = "Factories/Game Mode Factory")]
public class GameModeFactory : ScriptableObject
{
    [SerializeField] private OneVsOneOnlineGame _oneVsOneOnlineGame;
    [SerializeField] private TrainingGame _trainingGame;

    public Game GetGame(GameMode gameMode)
    {
        switch(gameMode)
        {
            case GameMode.OneVsOneOnline:
                return _oneVsOneOnlineGame;
            case GameMode.Training:
                return _trainingGame;
        }
        return null;
    }
}