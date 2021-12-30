using System;
using MyGame.Managers;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Views;

public class SettingsView : View
{
    [SerializeField] private Button backButton;
    [SerializeField] private Button enterButton;
    [SerializeField] private Toggle postProcessingToggle;
    [SerializeField] private Toggle fxaaToggle;
    [SerializeField] private Toggle motionBlurToggle;
    [SerializeField] private Toggle snowToggle;
    [SerializeField] private TMP_Dropdown graphicsQuality;

    private MotionBlur _motionBlur;
    
    public InputField playerNameInput;
        
    public override void Initialize()
    {
        base.Initialize();

        if (GameManager.Instance.postProcessVolume.profile.TryGet<MotionBlur>(out var tmp))
        {
            _motionBlur = tmp;
        }

        ApplySavedSettings();
        
        backButton.onClick.AddListener(() =>
        {
            ViewManager.ShowLast();
        });
        enterButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetString("playername", playerNameInput.text);
        });
        postProcessingToggle.onValueChanged.AddListener((value) =>
        {
            GameManager.Instance.postProcessVolume.enabled = value;
            PlayerPrefs.SetInt("PostProcessing", Convert.ToInt32(value));
        });
        fxaaToggle.onValueChanged.AddListener((value) =>
        {
            GameManager.Instance.mainCamera.GetComponent<UniversalAdditionalCameraData>().antialiasing = value ? AntialiasingMode.FastApproximateAntialiasing : AntialiasingMode.None;
            PlayerPrefs.SetInt("Fxaa", Convert.ToInt32(value));
        });
        motionBlurToggle.onValueChanged.AddListener((value) =>
        {
            if (_motionBlur) _motionBlur.active = value;
            PlayerPrefs.SetInt("MotionBlur", Convert.ToInt32(value));
        });
        snowToggle.onValueChanged.AddListener((value) =>
        {
            var snow = RoomController.Instance.localPlayer.GetSnowEffect();
            snow.SetActive(value);
            PlayerPrefs.SetInt("SnowEffect", Convert.ToInt32(value));
        });

        graphicsQuality.onValueChanged.AddListener((value) =>
        {
            UniversalRenderPipelineAsset quality = value switch
            {
                0 => GameManager.Instance.lowQualityGraphic,
                1 => GameManager.Instance.highQualityGraphic,
                _ => GameManager.Instance.lowQualityGraphic
            };

            QualitySettings.renderPipeline = quality;
            PlayerPrefs.SetInt("GraphicsQuality", value);
        });
    }

    private void ApplySavedSettings()
    {
        GraphicsSettings.defaultRenderPipeline = GameManager.Instance.lowQualityGraphic;
        QualitySettings.renderPipeline = Convert.ToBoolean(PlayerPrefs.GetInt("GraphicsQuality")) ? GameManager.Instance.highQualityGraphic : GameManager.Instance.lowQualityGraphic;
        graphicsQuality.value = PlayerPrefs.GetInt("GraphicsQuality");
        
        GameManager.Instance.postProcessVolume.enabled = Convert.ToBoolean(PlayerPrefs.GetInt("PostProcessing"));
        postProcessingToggle.isOn = GameManager.Instance.postProcessVolume.enabled;

        GameManager.Instance.mainCamera.GetComponent<UniversalAdditionalCameraData>().antialiasing =
            Convert.ToBoolean(PlayerPrefs.GetInt("Fxaa"))
                ? AntialiasingMode.FastApproximateAntialiasing
                : AntialiasingMode.None;
        fxaaToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("Fxaa"));

        try
        {
            _motionBlur.active = Convert.ToBoolean(PlayerPrefs.GetInt("MotionBlur"));
            motionBlurToggle.isOn = _motionBlur.active;
        }
        catch (Exception e)
        {
            Debug.LogError("No motion blur in volume profile");
        }
        
        // var snow = RoomController.Instance.localPlayer.GetSnowEffect();
        // snow.SetActive(Convert.ToBoolean(PlayerPrefs.GetInt("SnowEffect")));
        // snowToggle.isOn = Convert.ToBoolean(PlayerPrefs.GetInt("SnowEffect"));
    }

    public override void Show()
    {
        base.Show();
        playerNameInput.text = PlayerPrefs.GetString("playername");
    }
}
