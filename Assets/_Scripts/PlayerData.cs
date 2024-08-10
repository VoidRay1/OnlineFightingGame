using System;
using Unity.Services.Friends.Models;
using UnityEngine;

[Serializable]
public class PlayerData
{
    public string Id { get; private set; }
    public string Name { get; private set; }
    public Availability Availability { get; private set; }
    public Texture2D Avatar { get; private set; }
    public string Activity;
    public string AvatarId;

    public PlayerData(string id, string name)
    {
        Id = id;
        Name = name;
    }

    public PlayerData(string id, string name, string activity)
    {
        Id = id;
        Name = name;
        Activity = activity;
    }

    public PlayerData(string id, string name, Texture2D avatar)
    {
        Id = id;
        Name = name;
        Avatar = avatar;
    }

    public PlayerData(string id, string name, Availability availability, string activity)
    {
        Id = id;
        Name = name;
        Availability = availability;
        Activity = activity;
    }
}