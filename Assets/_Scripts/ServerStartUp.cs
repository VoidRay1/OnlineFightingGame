#if !SERVER
using System;
using System.Threading.Tasks;
using Unity.Services.Core;
using Unity.Services.Multiplay;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ServerStartUp : MonoBehaviour
{
    private async void Start()
    {
        await TryInitializeUnityServices();
        await TryStartServer();
        SceneManager.LoadSceneAsync(Constants.Scenes.MainMenu);
    }

    private async Task TryInitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (ServicesInitializationException e)
        {
            Debug.LogError(e.Message);
        }
    }

    private async Task TryStartServer()
    {
        try
        {
            await MultiplayService.Instance.StartServerQueryHandlerAsync(Constants.MaxPlayers, "n/a", "n/a", "0", "n/a");
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }
}
#endif