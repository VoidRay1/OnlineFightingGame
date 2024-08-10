using UnityEditor;
using UnityEngine;

public class GameCloser
{
    private GroupCreator _groupCreator;
    private PlayerData PlayerData => GameContext.Instance.PlayerData;

    public GameCloser(GroupCreator groupCreator)
    {
        _groupCreator = groupCreator;
    }

    public void CloseGame()
    {
        _groupCreator.TryLeaveGroup(PlayerData);
#if (UNITY_EDITOR)
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}