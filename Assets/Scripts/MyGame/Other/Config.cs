using System;
using System.IO;
using UnityEngine;

[Serializable]
public class Config
{
    public int pickUpBonusCoins = 1;
    public int startAmmo = 3;
    public int startHealth = 3;
    public float startSpeed = 10f;
    public float strafeSpeed = 6f;
    public float acceleration = 0.1f;
}

public static class GameSettings
{
    public static Config Config = new Config();
    private static readonly string Path = Application.dataPath + "/Resources" + "/Config.json";

    private static void CreateDefaultSettings()
    {
        string json = JsonUtility.ToJson(Config);
        File.WriteAllText(Path, json);
    }

    public static void GetSettingsFromFile()
    {
        if (!File.Exists(Path))
        {
            CreateDefaultSettings();
            return;
        }

        string json = File.ReadAllText(Path);
        Config = JsonUtility.FromJson<Config>(json);
    }
}
