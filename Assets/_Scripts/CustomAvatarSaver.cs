using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using DB = UnityEngine.Debug;

public class CustomAvatarSaver
{
    private const string AvatarsDirectory = "Avatars";
    private readonly AvatarsDataList _avatarsDataList;
    private ISaveService SaveService => GameContext.Instance.SaveService;

    public CustomAvatarSaver(AvatarsDataList avatarsDataList)
    {
        _avatarsDataList = avatarsDataList;
    }

    public AvatarData SaveAvatar(string fileName, Texture2D texture)
    {
        AvatarData avatarData = null;
        byte[] textureBytes = texture.EncodeToPNG();
        fileName = DateTime.Now.ToFileTime() + "_" + fileName;
        string filePath = BuildFilePath(fileName);
        if (IsAvatarsDirectoryExists() == false)
        {
            Directory.CreateDirectory(BuildFilePath());
        }
        try
        {
            avatarData = new AvatarData { Id = Guid.NewGuid().ToString(), Path = filePath };
            _avatarsDataList.AvatarsData.Add(avatarData);
            SaveService.Save(Constants.AvatarsDataFileName, _avatarsDataList);
            File.WriteAllBytes(filePath, textureBytes);
        }
        catch (UnauthorizedAccessException)
        {
            return null;
        }
        return avatarData;
    }

    public void DeleteAvatar(AvatarData avatarData)
    {
        if (_avatarsDataList.AvatarsData.Contains(avatarData))
        {
            _avatarsDataList.AvatarsData.Remove(avatarData);
            SaveService.Save(Constants.AvatarsDataFileName, _avatarsDataList);
            File.Delete(avatarData.Path);
        }
    }
    
    public void TryLoadAvatar(AvatarData avatarData)
    {
        if (avatarData == null)
        {
            return;
        }
        if (File.Exists(avatarData.Path))
        {
            try
            {
                byte[] avatarBytes = File.ReadAllBytes(avatarData.Path);
                Texture2D texture = new Texture2D(0, 0);
                avatarData.Texture = texture;
                texture.LoadImage(avatarBytes);
            }
            catch (FileLoadException e)
            {
                DB.LogError(e);
            }
        }
    }

    public List<AvatarData> LoadAllAvatars()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        List<AvatarData> avatars = new List<AvatarData>(_avatarsDataList.AvatarsData.Count);
        foreach (var avatarData in _avatarsDataList.AvatarsData)
        {
            TryLoadAvatar(avatarData);
            avatars.Add(avatarData);
        }
        sw.Stop();
        DB.Log(sw.ElapsedMilliseconds);
        return avatars;
    }

    private bool IsAvatarsDirectoryExists()
    {
        string filePath = BuildFilePath();
        if (Directory.Exists(filePath))
        {
            return true;
        }
        return false;
    }

    private string BuildFilePath(string fileName = "")
    {
        return Path.Combine(Application.persistentDataPath, AvatarsDirectory, fileName);
    }
}