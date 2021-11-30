using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    public static float CurrentVolume => AudioListener.volume;
    public float globalVolume => AudioSettings.globalVolume;
    
    [SerializeField] AudioSource musicSource, uiSource, inGameSource;
    [SerializeField] AudioMixer uiMixer, musicMixer, inGameMixer;

    [SerializeField] AudioClip clickSfx;
    [SerializeField] AudioClip pickUpSfx;
    [SerializeField] AudioClip loseSfx;
    [SerializeField] AudioClip winSfx;
    [SerializeField] AudioClip boomSfx;
    [SerializeField] AudioClip fireSfx;
    [SerializeField] AudioClip music;
    [SerializeField] AudioClip hitSfx;
    [SerializeField] AudioClip coinPickUpSfx;

    private Coroutine _fadeCoroutine;

    private void Awake()
    {
        InitializeSingleton();
        
        inGameSource.outputAudioMixerGroup = inGameMixer.FindMatchingGroups("Master")[0];
        
        AudioSettings.Init();
        AudioListener.volume = AudioSettings.globalVolume;
        FadeMixerGroup(musicMixer, AudioSettings.musicVolume);
        FadeMixerGroup(uiMixer, AudioSettings.uiVolume);
        
        EventHub.audioSettingsChanged += OnAudioSettingsChanged;
        EventHub.gamePaused += OnGamePaused;
        EventHub.gameOvered += OnGameOvered;
    }

    private void OnAudioSettingsChanged()
    {
        FadeMixerGroup(musicMixer, AudioSettings.musicVolume);
        FadeMixerGroup(uiMixer, AudioSettings.uiVolume);
    }

    private void OnGameOvered()
    {
        FadeMixerGroup(inGameMixer, AudioSettings.globalVolume * .5f);
        FadeMixerGroup(musicMixer, AudioSettings.musicVolume * .5f);
        PlayLose();
    }

    private void OnGamePaused(bool paused)
    {
        if(_fadeCoroutine != null) StopCoroutine(_fadeCoroutine);

        FadeMixerGroup(inGameMixer, paused ? globalVolume * .5f : globalVolume, duration: .5f);
    }

    private void FadeMixerGroup(AudioMixer audioMixer, float targetVolume, string exposedParam = "volume", float duration = 1f)
    {
        _fadeCoroutine = StartCoroutine(MixerGroupFader.StartFade(audioMixer, exposedParam, duration, targetVolume));
    }

    public void PlayBoom()
    {
        inGameSource.PlayOneShot(boomSfx);
    }

    public void PlayClick()
    {
        uiSource.PlayOneShot(clickSfx);
    }

    public void PlayLose() 
    {
        uiSource.PlayOneShot(loseSfx);
    }

    public void PlayWin()
    {
        uiSource.PlayOneShot(winSfx);
    }

    public void PlayPickUp()
    {
        uiSource.PlayOneShot(pickUpSfx);
    }

    public void PlayMusic()
    {
        musicSource.clip = music;
        musicSource.Play();
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void PlayHit()
    {
        uiSource.PlayOneShot(hitSfx);
    }

    public void PlayCoinPickUp()
    {
        uiSource.PlayOneShot(coinPickUpSfx);
    }

    public void PlayFire(AudioSource audioSource, AudioClip shootSound)
    {
        audioSource.PlayOneShot(shootSound);
    }

    private void OnDestroy()
    {
        StopAllCoroutines();
        EventHub.audioSettingsChanged -= OnAudioSettingsChanged;
        EventHub.gamePaused -= OnGamePaused;
        EventHub.gameOvered -= OnGameOvered;
    }

    public void PreRestartGame()
    {
        StopMusic();
        AudioListener.volume = AudioSettings.globalVolume;
        FadeMixerGroup(musicMixer, AudioSettings.musicVolume);
        FadeMixerGroup(uiMixer, AudioSettings.uiVolume);
    }
}
