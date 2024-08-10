using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;

public class StartupSceneEntryPoint : MonoBehaviour
{
    private readonly List<IInitializator> _initializators = new List<IInitializator>
    {
        new AuthenticationInitializator(),
        new ServicesInitializator(),
        new GameDataInitializator()
    };

    private async void Start()
    {
#if SERVER == false
        StartCoroutine(InitializeLocalizationSettings());
        await TryInitializeUnityServices();
        AuthenticationService.Instance.SignInFailed += SignInFailed;
        AuthenticationService.Instance.Expired += TokenExpired;
        foreach (var initializator in _initializators)
        {
            await initializator.Initialize();
        }
#else
        GameContext.Instance.InitializeGameControls();
        await TryInitializeUnityServices();
        SceneManager.LoadSceneAsync(Constants.Scenes.MainMenu);
#endif
    }

    private void TokenExpired()
    {
        print("Token expired");
    }

    private void SignInFailed(RequestFailedException obj)
    {
        print(obj);
    }

    private async Task TryInitializeUnityServices()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch(ServicesInitializationException e)
        {
            Debug.LogError(e.Message);
        }
    }

    private IEnumerator InitializeLocalizationSettings()
    {
        yield return LocalizationSettings.InitializationOperation;
    }

    private void OnDestroy()
    {
        AuthenticationService.Instance.SignInFailed -= SignInFailed;
        AuthenticationService.Instance.Expired -= TokenExpired;
    }
}