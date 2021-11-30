using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;

public static class AudioSettings
{
    public static float globalVolume;
    public static float uiVolume;
    public static float musicVolume;

    public static void Init()
    {
        globalVolume = PlayerPrefs.HasKey("globalvolume") ? PlayerPrefs.GetFloat("globalvolume") : .5f;
        uiVolume = PlayerPrefs.HasKey("uivolume") ? PlayerPrefs.GetFloat("uivolume") : .5f;
        musicVolume = PlayerPrefs.HasKey("musicvolume") ? PlayerPrefs.GetFloat("musicvolume") : .5f;
    }

    public static void Save()
    {
        PlayerPrefs.SetFloat("globalvolume", globalVolume);
        PlayerPrefs.SetFloat("uivolume", uiVolume);
        PlayerPrefs.SetFloat("musicvolume", musicVolume);

        EventHub.OnAudioSettingsChanged();
    }

    public static void SetGlobalVolume(float vol)
    {
        globalVolume = vol;
        AudioListener.volume = globalVolume;
    }
    
    public static void SetUiVolume(float vol)
    {
        uiVolume = vol;
    }
    
    public static void SetMusicVolume(float vol)
    {
        musicVolume = vol;
    }
}
