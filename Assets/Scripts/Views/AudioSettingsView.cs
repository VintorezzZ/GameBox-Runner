using System.Collections;
using System.Collections.Generic;
using MyGame.Managers;
using UnityEngine;
using UnityEngine.UI;
using Views;

public class AudioSettingsView : View
{
    [SerializeField] private Button backButton;
    [SerializeField] private Slider globalVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;


    public override void Initialize()
    {
        backButton.onClick.AddListener(() =>
        {
            AudioSettings.Save();
            ViewManager.ShowLast();
        });
        
        globalVolumeSlider.onValueChanged.AddListener(AudioSettings.SetGlobalVolume);
        uiVolumeSlider.onValueChanged.AddListener(AudioSettings.SetUiVolume);
        musicVolumeSlider.onValueChanged.AddListener(AudioSettings.SetMusicVolume);
    }

    public override void Show()
    {
        base.Show();
        
        globalVolumeSlider.value = AudioSettings.globalVolume;
        uiVolumeSlider.value = AudioSettings.uiVolume;
        musicVolumeSlider.value = AudioSettings.musicVolume;
    }
}
