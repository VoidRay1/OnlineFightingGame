using System;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public class AuthenticationInitializator : IInitializator
{
    public async Task Initialize()
    {
        await TrySignInAnonymouslyAsync();
    }

    private async Task TrySignInAnonymouslyAsync()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            throw new Exception("Unity Services are not initialized");
        }
        try
        {
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
        }
        Debug.Log($"User authorized: {AuthenticationService.Instance.IsAuthorized}");
        Debug.Log($"Token expired: {AuthenticationService.Instance.IsExpired}");
        Debug.Log($"User signed in: {AuthenticationService.Instance.IsSignedIn}");
    }
}