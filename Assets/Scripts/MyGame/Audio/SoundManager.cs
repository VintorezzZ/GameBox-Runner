using System.Collections;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Audio;
using Utils;

public class SoundManager : SingletonBehaviour<SoundManager>
{
    public static float CurrentVolume => AudioListener.volume;
    public float globalVolume => AudioSettings.globalVolume;
    
    [SerializeField] AudioSource musicSource, uiSource, inGameSource, footStepsSource, coinPickUpSource;
    [SerializeField] AudioMixer uiMixer, musicMixer, inGameMixer;

    [SerializeField] AudioClip music;
    [SerializeField] AudioClip clickSfx;
    [SerializeField] AudioClip loseSfx;
    
    [SerializeField] AudioClip coinPickUpSfx;
    [SerializeField] AudioClip cookieManPickUpSfx;
    
    [SerializeField] AudioClip rocketPickUpSfx;
    [SerializeField] AudioClip rocketLoopSFX;
    [SerializeField] AudioClip rocketEndSFX;
    [SerializeField] AudioClip rocketStartVox;
    
    [SerializeField] AudioClip jumpStartSFX;
    [SerializeField] AudioClip[] jumpVoxes;    
    
    [SerializeField] AudioClip slideSFX;
    [SerializeField] AudioClip slideVox;
    

    private Coroutine _fadeCoroutine;
    private Timer _rocketLoopTimer = new Timer();
    private Timer _pickUpCoinTimer = new Timer();

    private bool _gameStarted;
    
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
        EventHub.bonusRocketPickedUp += OnRocketPickUp;
        EventHub.coinsChanged += OnCoinPickUp;
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
        _gameStarted = false;
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

    // public void PlayBoom()
    // {
    //     inGameSource.PlayOneShot(boomSfx);
    // }

    public void PlayClick()
    {
        uiSource.PlayOneShot(clickSfx);
    }

    public void PlayLose() 
    {
        uiSource.PlayOneShot(loseSfx);
    }

    // public void PlayPickUp()
    // {
    //     uiSource.PlayOneShot(pickUpSfx);
    // }

    public void PlayMusic()
    {
        musicSource.clip = music;
        musicSource.Play();
        _gameStarted = true;
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // public void PlayHit()
    // {
    //     uiSource.PlayOneShot(hitSfx);
    // }
    private float _lastCoinPickUpTime;
    private float _coinPickUpDelay = 0.7f;
    private void OnCoinPickUp(int nothing)
    {
        if(!_gameStarted)
            return;
        
        if (Time.time > _lastCoinPickUpTime + _coinPickUpDelay)
        {
            coinPickUpSource.pitch = 1;
            _lastCoinPickUpTime = Time.time;
        }
        else
        {
            coinPickUpSource.pitch += 0.1f;
        }
        
        coinPickUpSource.PlayOneShot(coinPickUpSfx);
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
        EventHub.bonusRocketPickedUp -= OnRocketPickUp;
        EventHub.coinsChanged -= OnCoinPickUp;
    }

    public void PreRestartGame()
    {
        StopMusic();
        AudioListener.volume = AudioSettings.globalVolume;
        FadeMixerGroup(musicMixer, AudioSettings.musicVolume);
        FadeMixerGroup(uiMixer, AudioSettings.uiVolume);
    }

    public void OnRocketPickUp()
    {
        uiSource.PlayOneShot(rocketPickUpSfx);
        uiSource.PlayOneShot(rocketStartVox);
        
        StopCoroutine(RocketPickUpLoop());
        StartCoroutine(RocketPickUpLoop());
    }

    private IEnumerator RocketPickUpLoop()
    {
        if(_rocketLoopTimer.IsStarted)
            _rocketLoopTimer.Stop();
        
        _rocketLoopTimer.Start();
        uiSource.clip = rocketLoopSFX;
        uiSource.loop = true;
        uiSource.Play();

        yield return new WaitForSeconds(5);

        uiSource.Stop();
        uiSource.loop = false;
        uiSource.clip = null;
        uiSource.PlayOneShot(rocketEndSFX);
        _rocketLoopTimer.Stop();
    }

    public void PlayCookieManPickUp()
    {
        uiSource.PlayOneShot(cookieManPickUpSfx);
    }

    public void PlayJump()
    {
        uiSource.PlayOneShot(jumpStartSFX);
        
        var clip = jumpVoxes[Random.Range(0, jumpVoxes.Length)];
        uiSource.PlayOneShot(clip);
    }

    public void PlaySlide()
    {
        uiSource.PlayOneShot(slideVox);
        uiSource.PlayOneShot(slideSFX);
    }

    public void PlayStrafe()
    {
        uiSource.PlayOneShot(jumpStartSFX);
    }
    
    public void PlayFootSteps()
    {
        if(!footStepsSource.isPlaying)
            footStepsSource.Play();
    }
    public void StopFootSteps()
    {
        if(footStepsSource.isPlaying)
            footStepsSource.Stop();
    }
}
