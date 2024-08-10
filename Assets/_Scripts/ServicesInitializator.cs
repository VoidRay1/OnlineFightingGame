using System;
using System.Threading.Tasks;
using Unity.Services.Friends;
using Unity.Services.Friends.Exceptions;
using Unity.Services.Multiplay;
using UnityEngine;

public class ServicesInitializator : IInitializator
{
    public async Task Initialize()
    {
        await TryInitializeFriendsService();
    }

    private async Task TryInitializeFriendsService()
    {
        try
        {
            await FriendsService.Instance.InitializeAsync();
        }
        catch (FriendsServiceException e)
        {
            Debug.LogException(e);
        }
    }
}