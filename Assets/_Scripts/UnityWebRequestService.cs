using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class UnityWebRequestService : MonoBehaviour
{
    private Action<string, Texture2D> _onSuccess;
    private Action _onFail;

    public void GetTexture(string path, Action<string, Texture2D> onSuccess, Action onFail)
    {
        _onSuccess = onSuccess;
        _onFail = onFail;
        StartCoroutine(SendWebRequestTexture(path));
    }

    public IEnumerator SendWebRequestTexture(string path)
    {
        using (UnityWebRequest webRequest = UnityWebRequestTexture.GetTexture(path))
        {
            yield return webRequest.SendWebRequest();
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                _onFail?.Invoke();
            }
            else
            {
                var texture = DownloadHandlerTexture.GetContent(webRequest);
                _onSuccess?.Invoke(path, texture);
            }
        }
    }
}