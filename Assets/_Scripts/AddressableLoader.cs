using System.Threading.Tasks;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class AddressableLoader
{
    private GameObject _cachedGameObject;

    public async Task<T> Load<T>(string assetId, Transform parent = null)
    {
        var operation = Addressables.InstantiateAsync(assetId, parent);
        _cachedGameObject = await operation.Task;
        if (_cachedGameObject.TryGetComponent(out T component) == false)
        {
            throw new NullReferenceException($"{typeof(T)} does not exist on game object");
        }
        return component;
    }

    public async Task<T> Load<T>(string assetId, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        var operation = Addressables.InstantiateAsync(assetId, position, rotation, parent);
        _cachedGameObject = await operation.Task;
        if (_cachedGameObject.TryGetComponent(out T component) == false)
        {
            throw new NullReferenceException($"{typeof(T)} does not exist on game object");
        }
        return component;
    }

    public void Unload(GameObject gameObject)
    {
        if (_cachedGameObject == null)
        {
            return;
        }
        gameObject.SetActive(false);
        Addressables.ReleaseInstance(gameObject);
    }

    public void UnloadCachedGameObject()
    {
        if (_cachedGameObject == null)
        {
            return;
        }
        _cachedGameObject.SetActive(false);
        Addressables.ReleaseInstance(_cachedGameObject);
        _cachedGameObject = null;
    }
}